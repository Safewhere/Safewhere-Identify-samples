using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Safewhere.External.Interceptors;

namespace Safewhere.External.Samples
{
    /// <summary>
    /// the interceptor will do the followings:
    /// - Display a UI which asks for a security code. All allowed security codes can be hardcoded.
    /// - After a user enters a code and submits, check if the code is valid. 
    /// If yes, proceed to the next login step. 
    /// If no, display *another* view which tells: "The code you entered is invalid. Please enter a valid code below."
    /// </summary>
    public class SocialSecurityNumberConfirmationInterceptorService : IAuthenticationInterceptorService
    {
        /// <summary>
        /// Hardcode of valid social security number for demo
        /// </summary>
        private List<string> ValidNumbers = new List<string>
        {
            "1122", "1234", "6678", "0601", "2010"
        };

        public ActionResult Intercept(System.Web.Mvc.ControllerContext cc, System.Security.Claims.ClaimsPrincipal principal, IDictionary<string, string> input, string contextId, string viewName)
        {
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

            if (string.IsNullOrEmpty(viewName))
            {
                viewName = "SocialSecurityNumberConfirmationView";
            }

            var viewResult = new ViewResult
            {
                ViewName = viewName,
                ViewData =
                {
                    Model = new ModelWithContextId
                                {
                                    ContextId = contextId
                                }
                }
            };

            return viewResult;
        }

        public ActionResult OnPostBack(System.Web.Mvc.ControllerContext cc, System.Security.Claims.ClaimsPrincipal principal, IDictionary<string, string> input, string contextId, string viewName)
        {
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

            //Get social number from input tag in the view (<input type='text' name='socialnumber'/>)
            var socialnumber = string.Empty;
            var valueProviderResult = cc.Controller.ValueProvider.GetValue("socialnumber");
            if (valueProviderResult != null
                && !string.IsNullOrEmpty(valueProviderResult.AttemptedValue))
                socialnumber = valueProviderResult.AttemptedValue;

            //Verify the number
            if (!ValidNumbers.Any(n => n == socialnumber.Trim()))
            {
                return Intercept(cc, principal, input, contextId, viewName);
            }

            AddConnectionEntityIdentifiers(cc, principal);
            return null;
        }

        public IEnumerable<string> MustHaveInputKeys
        {
            get { return new List<string>(); }
        }

        private void AddConnectionEntityIdentifiers(ControllerContext cc, ClaimsPrincipal claimsPrincipal)
        {
            ClaimsIdentity identity = (ClaimsIdentity)claimsPrincipal.Identity;
            var epService = new PassiveContextService(cc.HttpContext);
            identity.AddClaim(new Claim("urn:SocialSecurityNumberConfirmationInterceptorService:AuthenticationConnectionEntityId", epService.AuthenticationConnectionEntityId));
            identity.AddClaim(new Claim("urn:SocialSecurityNumberConfirmationInterceptorService:ProtocolConnectionEntityId", epService.ProtocolConnectionEntityId));
        }
    }
}
