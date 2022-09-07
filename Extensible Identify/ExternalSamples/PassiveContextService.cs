using Safewhere.External.Model;
using System;
using System.Web;

namespace Safewhere.External.Samples
{
    /// <summary>
    /// A helper service that demonstrates how to access ISessionLoginContext and IIdentifyRequestInformation
    /// You can then use extension methods of those two objects to access other important data.
    /// For how to access such data without using extension methods, you can visit https://github.com/Safewhere/Safewhere-Identify-samples/tree/4773ab981b5ee4a7c370b92750c18697177dd7b4
    /// </summary>
    public class PassiveContextService
    {
        private const string TemporaryContextKey = "ici_TemporaryProtocolContext";
        private const string RequestInformationKey = "ici_RequestInformation";

        private readonly HttpContextBase httpContext;

        /// <summary>
        /// Instantiates a new IdentifyEndpointContextService object
        /// </summary>
        /// <param name="httpContext">HttpContext object</param>
        public PassiveContextService(HttpContextBase httpContext)
        {
            this.httpContext = httpContext ?? throw new ArgumentNullException("httpContext");
        }

        /// <summary>
        /// Returns the SessionLoginContext object
        /// </summary>
        public ISessionLoginContext SessionLoginContext
        {
            get
            {
                if (httpContext == null)
                {
                    throw new ArgumentNullException("httpContext");
                }

                if (httpContext.Items.Contains(TemporaryContextKey))
                    return (ISessionLoginContext)httpContext.Items[TemporaryContextKey];

                return null;
            }
        }

        /// <summary>
        /// Returns the RequestInformation object
        /// </summary>
        public IIdentifyRequestInformation RequestInformation
        {
            get
            {
                if (httpContext == null)
                {
                    throw new ArgumentNullException("httpContext");
                }

                if (httpContext.Items.Contains(RequestInformationKey))
                    return (IIdentifyRequestInformation)httpContext.Items[RequestInformationKey];

                return null;
            }
        }
    }
}
