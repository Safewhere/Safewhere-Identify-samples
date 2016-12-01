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
