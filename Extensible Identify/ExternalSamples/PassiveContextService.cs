using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Web;

namespace Safewhere.External.Samples
{
    /// <summary>
    /// A helper service that demonstrates how to access internal Identify context object using dynamic
    /// This class uses dynamic whose performance for the first invocation may be slow: https://stackoverflow.com/questions/7478387/how-does-having-a-dynamic-variable-affect-performance
    /// but subsequent invocations should be fairly faster.
    /// </summary>
    public class PassiveContextService
    {
        private const string EndpointContextKey = "endpointContext";
        private const string TemporaryContextKey = "ici_TemporaryProtocolContext";
        private static readonly Guid NullProtocolConnectionId = new Guid("{3D4A93FC-7AF4-4cf5-996C-25304D3FE15B}");
        private static readonly Guid NullAuthenticationConnectionId = new Guid("{DCDA0AC7-55BB-4665-886A-34595D457B3C}");

        private readonly HttpContextBase httpContext;

        /// <summary>
        /// Instantiates a new IdentifyEndpointContextService object
        /// </summary>
        /// <param name="httpContext">HttpContext object</param>
        public PassiveContextService(HttpContextBase httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException("httpContext");

            this.httpContext = httpContext;
        }

        /// <summary>
        /// Returns a full EndpointContext object. You can use VS' debugger window to examine its content.
        /// Notice that the EndpointContext object only exists after a login flow reaches to a certain point.
        /// Usually when you use this helper class in a claim transformation, a generic provider or an interceptor,
        /// the EndpointContext object should be available.
        /// </summary>
        public dynamic EndpointContext
        {
            get
            {
                return httpContext.Items[EndpointContextKey];
            }
        }

        /// <summary>
        /// Returns a context object that potentially contains a lot of 
        /// </summary>
        [Obsolete("Since Identify 5.12, it does not support any more, please use the SessionLoginContext property instead")]
        public dynamic TemporaryProtocolContext
        {
            get
            {
                return httpContext.Items[TemporaryContextKey];
            }
        }

        /// <summary>
        /// Returns a context object that potentially contains a lot of 
        /// </summary>
        public dynamic SessionLoginContext
        {
            get
            {
                if (httpContext == null)
                {
                    throw new ArgumentNullException("httpContext");
                }

                if (httpContext.Items.Contains(TemporaryContextKey))
                    return httpContext.Items[TemporaryContextKey];

                return null;
            }
        }

        /// <summary>
        /// Simple API to return id of a protocol connection
        /// </summary>
        public Guid ProtocolConnectionId
        {
            get
            {
                dynamic temporaryContext = SessionLoginContext;
                dynamic contextIdKey = temporaryContext.ContextIdKey;
                string contextId = contextIdKey.ContextId;
                Guid protocolConnectionId = contextIdKey.ProtocolConnectionId;
                return protocolConnectionId;
            }
        }

        /// <summary>
        /// Simple API to return entity id of a protocol connection. For OAuth 2.0/OpenId Connect connection, the clientid is returned
        /// </summary>
        public string ProtocolConnectionEntityId
        {
            get
            {
                dynamic temporaryContext = SessionLoginContext;
                dynamic contextIdKey = temporaryContext.ContextIdKey;
                string contextId = contextIdKey.ContextId;
                Guid protocolConnectionId = contextIdKey.ProtocolConnectionId;
                string protocolConnectionEntityId = contextIdKey.ProtocolConnectionEntityId;
                return protocolConnectionEntityId;
            }
        }

        /// <summary>
        /// Simple API to return id of an authentication connection.
        /// Notice that the authentication connection is only available after an Identity Provider has been chosen to do a login.
        /// </summary>
        public Guid AuthenticationConnectionId
        {
            get
            {
                return EndpointContext.AuthenticationContext.Connection.Id;
            }
        }

        /// <summary>
        /// Simple API to return entity id of an authentication connection. For OAuth 2.0/OpenId Connect connection, the clientid is returned
        /// Notice that the authentication connection is only available after an Identity Provider has been chosen to do a login.
        /// When an authentication connection is not available in the context, 
        /// </summary>
        public string AuthenticationConnectionEntityId
        {
            get
            {
                if (AuthenticationConnectionId == NullAuthenticationConnectionId)
                    return string.Empty;

                return EndpointContext.AuthenticationContext.Configuration.RetrieveEntityId();
            }
        }

        /// <summary>
        /// Simple API to return the RequesterId value of the scoping of a SAML 2.0 AuthnRequest message
        /// </summary>
        public IEnumerable<Uri> ScopingRequesterId
        {
            get
            {
                dynamic temporaryContext = SessionLoginContext;
                dynamic requestedAuthenticationContextModel = temporaryContext.RequestedAuthenticationContextModel;
                IEnumerable<Uri> requesterId = requestedAuthenticationContextModel.RequesterId;
                return requesterId;
            }
        }
    }
}
