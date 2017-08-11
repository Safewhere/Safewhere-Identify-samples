using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace Safewhere.Samples.STS.Common
{
    public class IdentifyStsCertificateEndpointHandler
    {
        public static SecurityToken GetSecurityToken(RequestSecurityTokenConfiguration rstConfiguration, SecurityToken bootstrapToken)
        {
            var certificateRst = new RequestSecurityToken
            {
                AppliesTo = new EndpointReference(rstConfiguration.AppliesTo),
                RequestType = RequestTypes.Issue,
                TokenType = "urn:oasis:names:tc:SAML:2.0:assertion",
                KeyType = KeyTypes.Symmetric,
                Issuer = new EndpointReference(rstConfiguration.StsEndpointAddress.Uri.AbsoluteUri),
            };
            if (bootstrapToken != null)
            {
                certificateRst.ActAs = new SecurityTokenElement(bootstrapToken);
            }
            if (rstConfiguration.ClientCertificate == null)
            {
                throw new IdentifyStsProcessException("Cannot execute negotiating token request to certificate endpoint withou client cerificate");
            }
            try
            {
                var stschannel = CreateStsChannel(rstConfiguration.ClientCertificate, rstConfiguration.StsEndpointAddress);
                return stschannel.Issue(certificateRst);
            }
            catch (Exception ex)
            {
                Logging.Instance.Error(ex, "There is an error responsed from ws-trust service.");
                throw;
            }
        }

        public static SecurityToken GetSecurityToken(RequestSecurityTokenConfiguration rstConfiguration)
        {
            return GetSecurityToken(rstConfiguration, null);
        }

        private static IWSTrustChannelContract CreateStsChannel(X509Certificate2 clientCertificate, EndpointAddress stsEndpointAddress)
        {
            var factory = new WSTrustChannelFactory(
                new IdentifyCertificateEndpointBinding(),
                stsEndpointAddress);
            factory.TrustVersion = TrustVersion.WSTrust13;

            if (factory.Credentials != null) factory.Credentials.ClientCertificate.Certificate = clientCertificate;

            return factory.CreateChannel();
        }
    }
}
