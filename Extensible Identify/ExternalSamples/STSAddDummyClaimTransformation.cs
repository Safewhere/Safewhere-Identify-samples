using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.ServiceModel;
using System.Web;
using Safewhere.External.ClaimsTransformation;

namespace Safewhere.External.Samples
{
    /// <summary>
    /// A transformation which protocol connection's entity identifier to a token
    /// </summary>
    public class STSAddDummyClaimTransformation : IExternalClaimsTransformation
    {
        public ClaimsPrincipal Transform(ClaimsPrincipal principal, IDictionary<string, string> inputs, IExternalClaimTransformationPipelineContext pipelineContext)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            AddConnectionEntityIdentifiers(principal);
            return principal;
        }

        private void AddConnectionEntityIdentifiers(ClaimsPrincipal claimsPrincipal)
        {
            ClaimsIdentity identity = (ClaimsIdentity)claimsPrincipal.Identity;
            if (HttpContext.Current != null)
            {
                var epService = new ActiveContextService(OperationContext.Current.IncomingMessageProperties);
                identity.AddClaim(new Claim("urn:STSAddDummyClaimTransformation:ProtocolConnectionId", epService.ProtocolConnectionId.ToString()));
                identity.AddClaim(new Claim("urn:STSAddDummyClaimTransformation:ProtocolConnectionEntityId", epService.ProtocolConnectionEntityId));
            }
        }
    }
}
