using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Safewhere.External;
using Safewhere.External.ClaimsTransformation;

namespace Safewhere.Samples.STS.GenericCredentialsValidator
{
    public class GenericProviderSampleClaimTransformation : IExternalClaimsTransformation
    {
        private readonly IIdentifyLogWriter _identifyLogWriter;

        public GenericProviderSampleClaimTransformation(IIdentifyLogWriter identifyLogWriter)
        {
            if (identifyLogWriter == null)
            {
                throw new ArgumentNullException(nameof(identifyLogWriter));
            }

            _identifyLogWriter = identifyLogWriter;
        }

        public ClaimsPrincipal Transform(ClaimsPrincipal principal, IDictionary<string, string> inputs,
            IExternalClaimTransformationPipelineContext pipelineContext)
        {
            if (principal == null)
            {
                throw new ArgumentNullException(nameof(principal));
            }
            if (inputs != null && inputs.ContainsKey("Should_Throw_Exception"))
            {
                throw new GenericProviderSampleException("Generic provider exception is thrown on GenericProviderSampleClaimTransformation as requested");
            }
            if (principal.Claims.Any(claim => claim.Value.Equals("ctexception")))
            {
                throw new GenericProviderSampleException("Generic provider exception is thrown on GenericProviderSampleClaimTransformation as requested");
            }
            principal.Identities.First()
                .AddClaim(new Claim("a_sample_static_claim_type", "a_sample_static_claim_value"));
            _identifyLogWriter.WriteInformation(
                $"A Static claim('{"a_sample_static_claim_type"}'.'{"a_sample_static_claim_value"}') is added");
            return principal;
        }
    }
}