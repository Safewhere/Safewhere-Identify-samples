using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Mvc;
using Safewhere.External.Interceptors;
using Safewhere.External.UserProfiles;

namespace Safewhere.External.Samples
{
    /// <summary>
    /// This sample is very simple: it returns a single profile which is created by using the input principal.
    /// </summary>
    [InterceptorDependencyServiceAttribute]
    public class SingleUserProfileService : IUserProfileService
    {
        public IEnumerable<UserProfile> GetUserProfiles(ControllerContext cc, ClaimsPrincipal principal, IDictionary<string, string> input, string contextId)
        {
            return new List<UserProfile>
                       {
                           new UserProfile
                               {
                                   Identity = principal.Identity.Name
                               }
                       };
        }

        public void Transform(ControllerContext cc, ClaimsPrincipal principal, IDictionary<string, string> input, string contextId,
                                         UserProfile selectedUserProfile)
        {
            // no-op
        }

        public bool ShowUserProfileSelectorWhenUserHasASingleProfile { get { return true; }  }

        public IEnumerable<string> MustHaveInputKeys { get { return new List<string>(); } }
    }
}