using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Safewhere.External.Authentication;
using Safewhere.External.Services;

namespace Safewhere.External.Samples
{
    public class TestModeGenericValidator : IGenericCredentialsValidator
    {
        public CredentialsValidationResult Validate(ControllerContext cc, IDictionary<string, string> inputs, IIdentifyLogWriter logWriter)
        {
            if (cc == null)
                throw new ArgumentNullException("cc");
            if (inputs == null)
                throw new ArgumentNullException("inputs");
            if (logWriter == null)
                throw new ArgumentNullException("logWriter");

            // Old ATP login code - execute as normal if connvert setting is false
            ValueProviderResult cprClaimType;
            if (!GetValue(cc, "CprClaimType", out cprClaimType))
            {
                return CreateShowLoginViewResult();
            }

            ValueProviderResult cprNumber;
            if (!GetValue(cc, "CprNumber", out cprNumber))
            {
                return CreateShowLoginViewResult();
            }

            ClaimsPrincipal principal = this.BuildPrincipal(cprClaimType, cprNumber);
            AddConnectionEntityIdentifiers(cc, principal);
            return new CredentialsValidationResult
            {
                ResultCode = CredentialsValidationResultCode.Success,
                ClaimsPrincipal = principal
            };
        }

        private ClaimsPrincipal BuildPrincipal(ValueProviderResult cprClaimType, ValueProviderResult cprNumber)
        {
            List<Claim> claims = new List<Claim>(1);
            claims.Add(new Claim(ClaimTypes.Name, cprNumber.AttemptedValue));
            claims.Add(new Claim(cprClaimType.AttemptedValue, cprNumber.AttemptedValue));

            ClaimsIdentity identity = new ClaimsIdentity(claims, "TestMode");
            return new ClaimsPrincipal(new ClaimsIdentity[] { identity });
        }

        private CredentialsValidationResult CreateShowLoginViewResult()
        {
            return new CredentialsValidationResult
            {
                ValidationActionResult = this.GetLoginView(),
                ResultCode = CredentialsValidationResultCode.MissingRequiredFields
            };
        }

        private ViewResult GetLoginView()
        {
            ViewResult result = new ViewResult();
            result.ViewName = "TestModeLogin";

            return result;
        }

        private static bool GetValue(ControllerContext context, string key, out ValueProviderResult vprName)
        {
            // TryGetValue doesn't exist in MVC4. It is said that GetValue returns null if the key doesn't exist
            vprName = context.Controller.ValueProvider.GetValue(key);

            return (vprName != null && !string.IsNullOrEmpty(vprName.AttemptedValue));
        }

        /// <summary>
        /// It is an example how to access protocol connection id and entityId in external modules
        /// </summary>
        /// <param name="cc"></param>
        /// <param name="claimsPrincipal"></param>
        private void AddConnectionEntityIdentifiers(ControllerContext cc, ClaimsPrincipal claimsPrincipal)
        {
            var epService = new PassiveContextService(cc.HttpContext);
            var requestInformation = epService.RequestInformation;

            ClaimsIdentity identity = (ClaimsIdentity)claimsPrincipal.Identity;
            identity.AddClaim(new Claim("urn:TestModeGenericValidator:AuthenticationConnectionEntityId", requestInformation.GetAuthenticationConnectionEntityId()));
            identity.AddClaim(new Claim("urn:TestModeGenericValidator:ProtocolConnectionEntityId", requestInformation.IdentifyLoginContext.GetProtocolConnectionEntityId()));
        }
    }
}
