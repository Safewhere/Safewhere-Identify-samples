using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Safewhere.External.Interceptors;
using Safewhere.External.Model;
using Safewhere.External.Services;

namespace Safewhere.External.Samples
{
    [ExternalTypeMetadataAttribute("Interactive partner selection for relying parties")]
    [PrimaryServiceType(typeof(IProtocolInterceptorService))]
    public class PartnerSelectionInterceptorService : IProtocolInterceptorService
    {
        private const string SourcePartnerClaimType = "SourcePartnerClaimType";
        private const string DestinationPartnerClaimType = "DestinationPartnerClaimType";
        private const string ValidValueRegEx = "ValidValueRegEx";

        public ActionResult Intercept(ControllerContext cc, ClaimsPrincipal principal, IIdentifyRequestInformation requestInformation, IDictionary<string, string> input, string contextId,
        string viewName)
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
                viewName = "PartnerSelectionView";
            }

            var model = new PartnerSelectionModel
            {
                ContextId = contextId
            };

            var partners = GetPartners(principal, input);
            if (partners.Count <= 1)
            {
                AddConnectionEntityIdentifiers(cc, principal, requestInformation);
                return null;
            }

            model.Partners = partners;
            var viewResult = new ViewResult
            {
                ViewName = viewName,
                ViewData =
                {
                    Model = model
                }
            };

            return viewResult;
        }

        private static List<string> GetPartners(ClaimsPrincipal principal, IDictionary<string, string> input)
        {
            string valueFilterRegEx = string.Empty;
            if (input.ContainsKey(ValidValueRegEx))
                valueFilterRegEx = input[ValidValueRegEx];

            string sourceClaimType = input[SourcePartnerClaimType];
            var partners = principal.Claims.Where(claim => claim.Type == sourceClaimType)
                .Select(claim => claim.Value);
            if (!string.IsNullOrEmpty(valueFilterRegEx))
            {
                var regEx = new Regex(valueFilterRegEx);
                partners = partners.Where(claim => regEx.IsMatch(claim));
            }

            return partners.Distinct()
             .ToList();
        }

        public ActionResult OnPostBack(ControllerContext cc, ClaimsPrincipal principal, IIdentifyRequestInformation requestInformation, IDictionary<string, string> input, string contextId,
            string viewName)
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

            var partner = string.Empty;
            var valueProviderResult = cc.Controller.ValueProvider.GetValue("partners");
            if (valueProviderResult != null
                && !string.IsNullOrEmpty(valueProviderResult.AttemptedValue))
                partner = valueProviderResult.AttemptedValue;

            var partners = GetPartners(principal, input);
            string destinationClaimType = input[DestinationPartnerClaimType];

            //Verify the number
            if (partners.All(n => n != partner.Trim()))
            {
                return Intercept(cc, principal, requestInformation, input, contextId, viewName);
            }

            ClaimsIdentity identity = ((ClaimsIdentity)principal.Identity);
            identity.AddClaim(new Claim(destinationClaimType, partner));
            AddConnectionEntityIdentifiers(cc, principal, requestInformation);
            return null;
        }

        public IEnumerable<string> MustHaveInputKeys
        {
            get
            {
                return new List<string>
                       {
                           "SourcePartnerClaimType",
                           "DestinationPartnerClaimType",
                           "ValidValueRegEx"
                       };
            }
        }

        private void AddConnectionEntityIdentifiers(ControllerContext cc, ClaimsPrincipal claimsPrincipal, IIdentifyRequestInformation requestInformation)
        {
            ClaimsIdentity identity = (ClaimsIdentity)claimsPrincipal.Identity;
            identity.AddClaim(new Claim("urn:PartnerSelectionInterceptorService:AuthenticationConnectionEntityId", requestInformation.GetAuthenticationConnectionEntityId()));
            identity.AddClaim(new Claim("urn:PartnerSelectionInterceptorService:ProtocolConnectionEntityId", requestInformation.IdentifyLoginContext.GetProtocolConnectionEntityId()));
        }
    }

    public class PartnerSelectionModel
    {
        public string ContextId { get; set; }

        public List<string> Partners { get; set; }
    }
}
