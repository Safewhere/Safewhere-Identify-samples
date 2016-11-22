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
    public class IdentifyStsUsernameEndpointHandler
    {
        public static SecurityToken GetSecurityToken(RequestSecurityTokenConfiguration rstConfiguration, SecurityToken bootstrapToken)
        {
            var upnRst = new RequestSecurityToken
            {
                AppliesTo = new EndpointReference(rstConfiguration.AppliesTo),
                RequestType = RequestTypes.Issue,
                TokenType = "urn:oasis:names:tc:SAML:2.0:assertion",
                KeyType = KeyTypes.Symmetric,
                Issuer = new EndpointReference(rstConfiguration.StsEndpointAddress.Uri.AbsoluteUri),
            };
            if (bootstrapToken != null)
            {
                upnRst.ActAs = new SecurityTokenElement(bootstrapToken);
            }

            if (string.IsNullOrEmpty(rstConfiguration.ClientUsername))
            {
                throw new IdentifyStsProcessException("Cannot execute negotiating token request to certificate endpoint withou client username or password");
            }

            if (rstConfiguration.Claims != null && rstConfiguration.Claims.Any())
            {
                upnRst.Claims.Dialect = "http://docs.oasis-open.org/wsfed/authorization/200706/authclaims";
                foreach (var claim in rstConfiguration.Claims)
                {
                    upnRst.Claims.Add(new RequestClaim(claim.Type, false, claim.Value));
                }
            }

            try
            {
                var stschannel = CreateStsChannel(rstConfiguration.ClientUsername, rstConfiguration.ClientPassword, rstConfiguration.StsEndpointAddress);
                return stschannel.Issue(upnRst);
            }
            catch (Exception ex)
            {
                Logging.Instance.Error(ex, "There is an error responsed from ws-trust service. The test will be failed");
                throw;
            }
        }

        private static IWSTrustChannelContract CreateStsChannel(string username, string password, EndpointAddress stsEndpointAddress)
        {
            var factory = new WSTrustChannelFactory(
                new IdentifyUsernameEndpointBinding(),
                stsEndpointAddress);
            factory.TrustVersion = TrustVersion.WSTrust13;

            if (factory.Credentials != null)
            {
                factory.Credentials.UserName.UserName = username;
                factory.Credentials.UserName.Password = password;
            }

            return factory.CreateChannel();
        }

        public static SecurityToken GetSecurityToken(RequestSecurityTokenConfiguration rstConfiguration)
        {
            return GetSecurityToken(rstConfiguration, null);
        }
    }
}
