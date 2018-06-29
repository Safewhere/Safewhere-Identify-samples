using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Safewhere.External.Interceptors;
using Safewhere.External.UserProfiles;

namespace Safewhere.External.Samples
{
    /// <summary>
    /// An LDAP implementation of IUserProfileService which is used by the "interactively select user profiles" feature of Identify.
    /// </summary>
    /// <remarks>
    /// InterceptorDependencyService attribute tells Identify that this is a dependency of the user profiles selection service.
    /// PrimaryServiceType attribute tells Identify that this service should be registered as an instance of IUserProfileService
    /// ExternalTypeMetadata attribute tells Identify that friendly name of this service is "Look up user profiles from a Ldap store"
    /// </remarks>
    [InterceptorDependencyServiceAttribute]
    [PrimaryServiceType(typeof(IUserProfileService))]
    [ExternalTypeMetadata("Look up user profiles from a Ldap store")]
    public class LdapUserProfileService : IUserProfileService
    {
        private readonly IIdentifyLdapStore identifyLdapStore;
        private readonly IIdentifyLogWriter logWriter;

        /// <summary>
        /// This sample has two dependencies to IIdentifyLdapStore and IIdentifyLogWriter. Their implementations are provided by Identify.
        /// </summary>
        /// <param name="identifyLdapStore">A service whose implementation is built in Identify. It is used to access LDAP stores</param>
        /// <param name="logWriter">A logger</param>
        public LdapUserProfileService(IIdentifyLdapStore identifyLdapStore, IIdentifyLogWriter logWriter)
        {
            if (identifyLdapStore == null)
            {
                throw new ArgumentNullException("identifyLdapStore");
            }
            if (logWriter == null)
            {
                throw new ArgumentNullException("logWriter");
            }

            this.identifyLdapStore = identifyLdapStore;
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

            if (!input.ContainsKey("identityAttributeName"))
            {
                // can write event with id to event log.
                logWriter.WriteError(9999, "The input key-value dictionary doesn't contain identity attribute name. That attribute name is needed to do a look up against a Ldap store");
                throw new ArgumentException(
                    "The input key-value dictionary doesn't contain identity attribute name. That attribute name is needed to do a look up against a Ldap store");
            }
            if (!input.ContainsKey("ldapwsServerName"))
            {
                throw new ArgumentException(
                    "The input key-value dictionary doesn't contain a ldapWS server name (whose key is ldapwsServerName). That ldapWS server name is needed to do a look up against a Ldap store");
            }

            string identityAttributeName = input["identityAttributeName"];
            string ldapwsServerName = input["ldapwsServerName"];
            // we need to know what user's attribute value should be used to compare against the identityattribute in Ldap
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

            // here we assume that attributes to fetch have keys which start with ldapattr_
            var additionalAttributes = input.Where(kvp => kvp.Key.StartsWith("ldapattr_")).Select(kvp => kvp.Value);

            IEnumerable<IDictionary<string, object>> users = identifyLdapStore.GetUsers(identityAttributeName,
                                                                                        identityValue,
                                                                                        ldapwsServerName,
                                                                                        additionalAttributes);
            List<UserProfile> userProfiles = new List<UserProfile>();
            foreach (var user in users)
            {
                // how to map a user object to a user profile depends on specific scenarios.
                // An easy way is to make hard-mapping between a ldap attribute and a userProfile like in this example
                // A more flexible way is to pass such a mapping to this method via the input parameter. The mapping is configured via a UI in Identify*Admin
                // For example:
                //      {prop_identity, samAccountName}
                //      {prop_email, mail}
                //      {prop_photourl, url}

                var userProfile = new UserProfile();
                userProfile.Identity = user["sAMAccountName"].ToString();
                const string displayNameAttr = "Display-Name";
                if (user.ContainsKey(displayNameAttr) && user[displayNameAttr] != null)
                {
                    userProfile.DisplayName = user[displayNameAttr].ToString();
                }
                const string mailAttr = "mail";
                if (user.ContainsKey(mailAttr) && user[mailAttr] != null)
                {
                    userProfile.Email = user[mailAttr].ToString();
                }
                const string photoUrlAttr = "url";
                if (user.ContainsKey(photoUrlAttr) && user[photoUrlAttr] != null)
                {
                    userProfile.PhotoUrl = user[photoUrlAttr].ToString();
                }

                userProfile.Attributes = user;  // finally, assign the "user" object to the Attributes property just in case the caller needs it

                userProfiles.Add(userProfile);
            }

            return userProfiles;
        }

        public void Transform(ControllerContext cc, ClaimsPrincipal principal, IDictionary<string, string> input, string contextId,
                                         UserProfile selectedUserProfile)
        {
            // this sample replaces the old identity with one picked from the selected profile
            ClaimsIdentity identity = ((ClaimsIdentity)principal.Identity);
            IEnumerable<Claim> nameClaims = identity.FindAll(identity.NameClaimType);
            string nameClaimType = identity.NameClaimType;
            foreach (var nameClaim in nameClaims)
            {
                identity.RemoveClaim(nameClaim);
            }

            identity.AddClaim(new Claim(nameClaimType, selectedUserProfile.Identity));


            // so what about the other attributes of the user? Two ways:
            //  1. Specify all the required attributes to get out on the GetUserProfiles method. Examine selectedUserProfile.Attributes for those attributes and convert them to claims
            //  2. A better way is to not fetch additional attributes in this step. Instead, use Identify ldap claim transformation rule.
        }

        public bool ShowUserProfileSelectorWhenUserHasASingleProfile { get { return false; } }

        public IEnumerable<string> MustHaveInputKeys
        {
            get { return new List<string> {"identityAttributeName", "ldapwsServerName"}; }
        }
    }
}
