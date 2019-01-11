using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using Safewhere.External.ClaimsTransformation;

namespace Safewhere.External.Samples
{
    public class BoostrapTokenTestClaimTransformation
    {
        private readonly IIdentifyLogWriter identifyLogWriter;

        public BoostrapTokenTestClaimTransformation(IIdentifyLogWriter identifyLogWriter)
        {
            this.identifyLogWriter = identifyLogWriter ?? throw new ArgumentNullException("identifyLogWriter");
        }

        public ClaimsPrincipal Transform(ClaimsPrincipal principal, IDictionary<string, string> inputs, IExternalClaimTransformationPipelineContext pipelineContext)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            try
            {
                var identity = principal.Identities.FirstOrDefault(x => x.BootstrapContext != null);

                if (identity == null)
                {
                    identifyLogWriter.WriteDebug("Identify having boostrap context is null");
                    return principal;
                }

                var bootstrapContext = identity.BootstrapContext as BootstrapContext;
                if (bootstrapContext == null)
                {
                    identifyLogWriter.WriteDebug("Boostrap context has invalid format");
                    return principal;
                }
                identifyLogWriter.WriteDebug("Boostrap context is valid");

            }
            catch (InvalidOperationException ex)   // this exception type is caught for illustration purpose only
            {
                // Ids from 4830 to 4899 are reserved for external components. Pick one that doesn't conflict with other external events in the same set up
                identifyLogWriter.WriteError(4830, ex);
            }

            return principal;
        }
    }
}
