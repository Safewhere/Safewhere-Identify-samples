using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Safewhere.Samples.STS.Common
{
    public class IdentifyUsernameEndpointBinding: Binding
    {
        public override BindingElementCollection CreateBindingElements()
        {
            var elements = new BindingElementCollection();
            elements.Clear();

            elements.Add(CreateSecurityBindingElement());
            elements.Add(CreateMessageEncodingBindingElement());
            elements.Add(CreateTransportBindingElement());
            return elements.Clone();
        }

        private BindingElement CreateTransportBindingElement()
        {
            var result = new HttpsTransportBindingElement
            {
                AuthenticationScheme = AuthenticationSchemes.Digest
            };

            return result;
        }

        private BindingElement CreateMessageEncodingBindingElement()
        {
            return new TextMessageEncodingBindingElement
            {
                ReaderQuotas = { MaxArrayLength = 0x200000, MaxStringContentLength = 0x200000 }
            };
        }

        private static SecurityBindingElement CreateSecurityBindingElement()
        {
            var result = SecurityBindingElement.CreateUserNameOverTransportBindingElement();
            result.MessageSecurityVersion =
                   MessageSecurityVersion
                       .WSSecurity11WSTrust13WSSecureConversation13WSSecurityPolicy12BasicSecurityProfile10;
            return result;
        }

        public override string Scheme
        {
            get
            {
                TransportBindingElement element = CreateBindingElements().Find<TransportBindingElement>();

                if (element == null)
                {
                    return string.Empty;
                }

                return element.Scheme;
            }
        }
    }
}
