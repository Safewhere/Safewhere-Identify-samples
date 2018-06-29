using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using Safewhere.External.ClaimsTransformation;

namespace Safewhere.External.Samples
{
    /// <summary>
    /// Similar to <see cref="AddDummyClaimTransformation"/>. Created to demonstrate the ability of Identify to handle multiple transformations.
    /// </summary>
    public class AnotherAddDummyClaimTransformation : IExternalClaimsTransformation
    {
        private readonly IIdentifyLogWriter identifyLogWriter;

        public AnotherAddDummyClaimTransformation(IIdentifyLogWriter identifyLogWriter)
        {
            if (identifyLogWriter == null)
            {
                throw new ArgumentNullException("identifyLogWriter");
            }

            this.identifyLogWriter = identifyLogWriter;
        }

        public ClaimsPrincipal Transform(ClaimsPrincipal principal, IDictionary<string, string> inputs, IExternalClaimTransformationPipelineContext pipelineContext)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("principal");
            }

            // At the time this sample code is written, Identify supports only one identity
            var type = GetType();
            foreach (var identity in principal.Identities)
            {
                identity.AddClaim(new Claim(type.FullName, type.AssemblyQualifiedName));
                identifyLogWriter.WriteInformation(string.Format(CultureInfo.InvariantCulture,
                                                                 "A new claim is added: [{0},{1}]", type.FullName,
                                                                 type.AssemblyQualifiedName));
            }

            return principal;
        }
    }
}