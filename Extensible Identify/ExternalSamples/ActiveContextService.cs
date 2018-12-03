using System;
using System.Collections.Generic;

namespace Safewhere.External.Samples
{
    /// <summary>
    /// A helper service that demonstrates how to access internal Identify context object using dynamic for active scenarios
    /// This class uses dynamic whose performance for the first invocation may be slow: https://stackoverflow.com/questions/7478387/how-does-having-a-dynamic-variable-affect-performance
    /// but subsequent invocations should be fairly faster.
    /// This helper class works for Identify 5.5++.
    /// </summary>
    public class ActiveContextService
    {
        private const string ProtocolConnectionEntityIdKey = "ProtocolConnectionEntityId";
        private const string ProtocolConnectionKey = "ProtocolConnection";

        private readonly IDictionary<string, object> messageProperties;

        /// <summary>
        /// Instantiates a new IdentifyEndpointContextService object
        /// </summary>
        /// <param name="messageProperties">HttpContext object</param>
        public ActiveContextService(IDictionary<string, object> messageProperties)
        {
            if (messageProperties == null)
                throw new ArgumentNullException("messageProperties");

            this.messageProperties = messageProperties;
        }
        
        /// <summary>
        /// Simple API to return id of a protocol connection
        /// </summary>
        public Guid ProtocolConnectionId
        {
            get
            {
                if (!messageProperties.ContainsKey(ProtocolConnectionKey))
                {
                    return Guid.Empty;
                }

                dynamic protocolConnection = messageProperties[ProtocolConnectionKey];
                return protocolConnection.Id;
            }
        }

        /// <summary>
        /// Simple API to return entity id of a protocol connection. For OAuth 2.0/OpenId Connect connection, the clientid is returned
        /// </summary>
        public string ProtocolConnectionEntityId
        {
            get
            {
                if (!messageProperties.ContainsKey(ProtocolConnectionEntityIdKey))
                {
                    return string.Empty;
                }

                return (string)messageProperties[ProtocolConnectionEntityIdKey];
            }
        }
    }
}
