using System;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Security;

namespace Safewhere.Samples.STS.Common
{
    public class IdentifyStsServiceCommon
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
            try
            {
                var stschannel = CreateStsChannel(rstConfiguration.ClientCertificate, rstConfiguration.StsEndpointAddress);
                return stschannel.Issue(certificateRst);
            }
            catch (Exception ex)
            {
                Logging.Instance.Error(ex, "There is an error responsed from ws-trust service. The test will be failed");
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
                new IdentifyCertificateMixedEndpointBinding(),
                stsEndpointAddress);
            factory.TrustVersion = TrustVersion.WSTrust13;

            if (factory.Credentials != null) factory.Credentials.ClientCertificate.Certificate = clientCertificate;

            return factory.CreateChannel();
        }

        public static T GetClaimAppService<T>(SecurityToken securityToken, string serviceAddress) where T : class
        {
            try
            {
                var binding =
                    new WS2007FederationHttpBinding(WSFederationHttpSecurityMode.TransportWithMessageCredential);
                binding.Security.Message.EstablishSecurityContext = false;
                binding.Security.Message.IssuedKeyType = SecurityKeyType.BearerKey;

                var factory = new ChannelFactory<T>(
                    binding,
                    new EndpointAddress(new Uri(serviceAddress), EndpointIdentity.CreateDnsIdentity("IdentifyDefaultSigning")));
                factory.Credentials.SupportInteractive = false;

                var channel = factory.CreateChannelWithIssuedToken(securityToken);
                return channel;
            }
            catch (Exception ex)
            {
                Logging.Instance.Error(ex, "Error while execute remote operation");
                throw;
            }
        }
    }
}
