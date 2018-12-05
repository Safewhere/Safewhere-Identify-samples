using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Web;
using Safewhere.External.ClaimsTransformation;

namespace Safewhere.External.Samples
{
    /// <summary>
    /// A transformation which adds its own .NET type name to ClaimsPrincipal
    /// </summary>
    public class AddDummyClaimTransformation : IExternalClaimsTransformation
    {
        private readonly IIdentifyLogWriter identifyLogWriter;

        public AddDummyClaimTransformation(IIdentifyLogWriter identifyLogWriter)
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

            try
            {
                var type = GetType();
                // each identity provider may add its own identity to the claims principal object
                // at the time the principal object is passed to this transformation rule, how many identities it contains depends on
                // what previous transformations it has gone through.
                // Normally speaking:
                // 1. After Identify receives a token from an upstream Idp or from the UserNamePassword login, it creates the principal with one identity.
                // 2. After that, Identify loads claims from the local store into another identity and adds it to the principal (2 identities so far)
                // 3. There is a setting called "Execute before loading claims from local store" in the claims transformation configuration page which controls
                // if a transformation should be executed before or after the second identity is added.
                // 4. Note 1: Other transformations may add more identities to the claims principal.
                // 5. Note 2: One can choose to add claims to one identity or to all identities depending on specific need.
                foreach (var identity in principal.Identities)
                {
                    identity.AddClaim(new Claim(type.FullName, type.AssemblyQualifiedName));
                    identifyLogWriter.WriteInformation(string.Format(CultureInfo.InvariantCulture,
                                                                     "A new claim is added: [{0},{1}]", type.FullName,
                                                                     type.AssemblyQualifiedName));
                }

                AddConnectionEntityIdentifiers(principal);
            }
            catch (InvalidOperationException ex)   // this exception type is caught for illustration purpose only
            {
                // Ids from 4830 to 4899 are reserved for external components. Pick one that doesn't conflict with other external events in the same set up
                identifyLogWriter.WriteError(4830, ex);
            }

            return principal;
        }

        private void AddConnectionEntityIdentifiers(ClaimsPrincipal claimsPrincipal)
        {
            ClaimsIdentity identity = (ClaimsIdentity)claimsPrincipal.Identity;
            if (HttpContext.Current != null)
            {
                var epService = new PassiveContextService(new HttpContextWrapper(HttpContext.Current));
                identity.AddClaim(new Claim("urn:AddDummyClaimTransformation:AuthenticationConnectionEntityId", epService.AuthenticationConnectionEntityId));
                identity.AddClaim(new Claim("urn:AddDummyClaimTransformation:ProtocolConnectionEntityId", epService.ProtocolConnectionEntityId));
            }
        }
    }
}
