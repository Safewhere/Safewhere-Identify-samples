using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using Safewhere.External.Interceptors;
using Safewhere.External.UserProfiles;

namespace Safewhere.External.Samples
{
    /// <summary>
    /// A local store implementation of IUserProfileService which is used by the "interactively select user profiles" feature of Identify.
    /// </summary>
    /// <remarks>
    /// InterceptorDependencyService attribute tells Identify that this is a dependency of the user profiles selection service.
    /// Here we don't use PrimaryBaseType. In this case, Identify will try to find the first interface, and then the first abstract class, which this component implements.
    /// ExternalTypeMetadata attribute tells Identify that friendly name of this service is "Look up user profiles from the local store"
    /// </remarks>
    [InterceptorDependencyServiceAttribute]
    [ExternalTypeMetadata("Look up user profiles from the local store")]
    public class LocalStoreUserProfileService : IUserProfileService
    {
        private readonly IIdentifyLocalStore identifyLocalStore;
        private readonly IIdentifyLogWriter logWriter;

        /// <summary>
        /// This sample has two dependencies to IIdentifyLdapStore and IIdentifyLogWriter. Their implementation are provided by Identify.
        /// </summary>
        /// <param name="identifyLocalStore">A service whose implementation is built in Identify. It is used to access Identify's local store</param>
        /// <param name="logWriter">A logger</param>
        public LocalStoreUserProfileService(IIdentifyLocalStore identifyLocalStore, IIdentifyLogWriter logWriter)
        {
            if (identifyLocalStore == null)
            {
                throw new ArgumentNullException("identifyLocalStore");
            }
            if (logWriter == null)
            {
                throw new ArgumentNullException("logWriter");
            }

            this.identifyLocalStore = identifyLocalStore;
            this.logWriter = logWriter;
        }

        public IEnumerable<UserProfile> GetUserProfiles(ControllerContext cc, ClaimsPrincipal principal, IDictionary<string, string> input, string contextId)
        {
            #region sanity checks
            if (cc == null)
            {
                throw new ArgumentNullException("cc");
            }
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (contextId == null)
            {
                throw new ArgumentNullException("contextId");
            }
            #endregion

            if (!input.ContainsKey("identityLocalClaimType"))
            {
                const string errorMessage =
                    "The input key-value dictionary doesn't contain identity claim name. That attribute name is needed to do a look up against a local user store";
                // can write event with id to event log.
                logWriter.WriteError(9999, errorMessage);
                throw new ArgumentException(errorMessage);
            }
            string identityAttributeName = input["identityLocalClaimType"];

            // we need to know what user's claim value should be used to compare against the claim type in local store
            // default value
            string identityValue = principal.Identity.Name;
            // check if another claim is specified whose value to be used to look up in Ldap
            if (input.ContainsKey("identityClaimType"))
            {
                Claim identityClaim = principal.Claims.FirstOrDefault(claim => claim.Type == input["identityClaimType"]);
                if (identityClaim != null)
                {
                    identityValue = identityClaim.Value;
                }
            }

            IEnumerable<IDictionary<string, object>> users = identifyLocalStore.GetUsers(identityAttributeName, identityValue);
            List<UserProfile> userProfiles = new List<UserProfile>();
            foreach (var user in users)
            {
                // how to map a user object to a user profile depends on specific scenarios.
                // An easy way is to make hard-mapping between a claim and a userProfile like in this example
                // A more flexible way is to pass such a mapping to this method via the input parameter. The mapping is configured via a UI in Identify*Admin
                // For example:
                //      {prop_identity, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name}
                //      {prop_email, http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress }
                //      {prop_photourl, urn:safewhere:photo:url}

                var userProfile = new UserProfile();
                // This sample assumes that the real username is stored with the claim type below
                userProfile.Identity = ((List<string>)user["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"]).First();

                // __IdentifyUserName is a reserved key for Identify's user name (aka display name)
                // The other two are __Organization and __Group
                const string displayNameAttr = "__IdentifyUserName";
                if (user.ContainsKey(displayNameAttr) && user[displayNameAttr] != null)
                {
                    userProfile.DisplayName = user[displayNameAttr].ToString();
                }

                // This sample assumes that the real email is stored with the claim type below
                const string mailAttr = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress";
                if (user.ContainsKey(mailAttr) && user[mailAttr] != null)
                {
                    userProfile.Email = GetClaimValue(user[mailAttr]);
                }
                const string photoUrlAttr = "urn:photourl";
                if (user.ContainsKey(photoUrlAttr) && user[photoUrlAttr] != null)
                {
                    userProfile.PhotoUrl = GetClaimValue(user[photoUrlAttr].ToString());
                }

                userProfile.Attributes = user;  // finally, assign the "user" object to the Attributes property just in case the caller needs it

                userProfiles.Add(userProfile);
            }

            return userProfiles;
        }

        /// <summary>
        /// Some claim type may have multiple values
        /// </summary>
        /// <param name="claimValue"></param>
        /// <returns></returns>
        private string GetClaimValue(object claimValue)
        {
            if (claimValue == null)
                return string.Empty;

            var multiValues = claimValue as IEnumerable<string>;
            if (multiValues != null)
            {
                return string.Join(",", multiValues);
            }

            return claimValue.ToString();
        }

        public void Transform(ControllerContext cc, ClaimsPrincipal principal, IDictionary<string, string> input, string contextId,
                              UserProfile selectedUserProfile)
        {
            // this sample replaces the old identity with one picked from the selected profile
            ClaimsIdentity identity = ((ClaimsIdentity)principal.Identity);
            Claim nameClaim = identity.FindFirst(identity.NameClaimType);
            string nameClaimType = identity.NameClaimType;
            identity.RemoveClaim(nameClaim);
            identity.AddClaim(new Claim(nameClaimType, selectedUserProfile.Identity));

            // so what about the other attributes of the user? Two ways:
            //  1. Specify all the required attributes to get out on the GetUserProfiles method. Examine selectedUserProfile.Attributes for those attributes and convert them to claims
            //  2. A better way is to not fetch additional attributes in this step. Instead, use claim transformation rules. Please note that
            //      if a user exists in local store, UserClaimTransformation rule will be responsible for adding claims from local store.
        }

        public bool ShowUserProfileSelectorWhenUserHasASingleProfile { get { return false; } }

        public IEnumerable<string> MustHaveInputKeys { get { return new List<string> { "identityClaimName" }; } }
    }
}