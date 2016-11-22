using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.Xml;

namespace Safewhere.Samples.STS.Common
{
    public class RequestSecurityTokenConfigurationSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        /// Creates a configuration section handler.
        /// </summary>
        public object Create(object parent, object configContext, XmlNode section)
        {
            var config = new RequestSecurityTokenConfiguration();

            config.LoadConfiguration(section);

            return config;
        }
    }
    /// <summary>
    /// Configuration for claimapp services
    /// </summary>
    public class RequestSecurityTokenConfiguration
    {
        /// <summary>
        /// A RST request model to Identify
        /// </summary>
        private const string SectionName = "SecurityTokenRequest";

        #region Properties

        /// <summary>
        /// Sts certificate endpoint
        /// </summary>
        public EndpointAddress StsEndpointAddress { get; private set; }

        /// <summary>
        /// Sts endpoint identity
        /// </summary>
        public string StsEndpointIdentity { get; private set; }

        /// <summary>
        /// Sts endpoint certificate
        /// </summary>
        public X509Certificate2 StsEndpointCertificate { get; private set; }

        /// <summary>
        /// AppliesTo
        /// </summary>
        public string AppliesTo { get; private set; }

        /// <summary>
        /// Client certificate
        /// </summary>
        public X509Certificate2 ClientCertificate { get; private set; }

        /// <summary>
        /// Client username, it will be available if there is no client certificate setting
        /// </summary>
        public string ClientUsername { get; private set; }

        /// <summary>
        ///  Client password, it will be available if there is no client certificate setting
        /// </summary>
        public string ClientPassword { get; private set; }

        /// <summary>
        /// Additional claims which will be sent to STS via RST
        /// </summary>
        public IList<Claim> Claims { get; private set; }
        #endregion

        #region Public

        /// <summary>
        /// Get the RST configuration
        /// </summary>
        public static RequestSecurityTokenConfiguration Get()
        {
            var config = (RequestSecurityTokenConfiguration)ConfigurationManager.GetSection(SectionName);

            return config;
        }

        /// <summary>
        /// Load configuration from web.config
        /// </summary>
        public void LoadConfiguration(XmlNode section)
        {
            if (section == null) throw new ArgumentNullException("section");
            StsEndpointIdentity = GetChildElementInnerText(section, "StsEndpointIdentity", true);
            StsEndpointAddress = CreateEndpointAddress(
                GetChildElementInnerTextAsUri(section, "StsEndpointAddress", true),
                StsEndpointIdentity);

            StsEndpointCertificate = GetChildElementInnerTextAsX509Certificate(section, "StsEndpointCertificate", true);

            AppliesTo = GetChildElementInnerText(section, "AppliesTo", true);

            var clientCrendentialElement = GetChildElementFromName(section, "ClientCredential", true);

            if (clientCrendentialElement.HasAttribute("username"))
            {
                ClientUsername = GetAttributeValueAsString(clientCrendentialElement, "username", true);
                ClientPassword = GetAttributeValueAsString(clientCrendentialElement, "password", true);
            }
            else
            {
                ClientCertificate = GetChildElementCertificateReferenceAsX509Certificate(clientCrendentialElement);
            }
            var claimsElement = GetChildElementFromName(section, "Claims", false);
            if (claimsElement != null)
            {
                var claimsListElement = GetChildrenElementsFromName(claimsElement, "Claim", false);
                Claims = GetAllAdditionalClaims(claimsListElement);
            }
            
        }
        #endregion

        #region Private

        private IList<Claim> GetAllAdditionalClaims(XmlNodeList claimsNodeList)
        {
            var claims = new List<Claim>();
            foreach (var claimNode in claimsNodeList)
            {
                var claimElement = (XmlElement) claimNode;
                var claimType = GetAttributeValueAsString(claimElement, "type", true);
                var claimValue = GetAttributeValueAsString(claimElement, "value", true);
                claims.Add(new Claim(claimType, claimValue));
            }

            return claims;
        }
        private static XmlNodeList GetChildrenElementsFromName(XmlNode parentElement, string elementName, bool required)
        {
            if (parentElement == null) throw new ArgumentNullException(nameof(parentElement));
            if (elementName == null) throw new ArgumentNullException(nameof(elementName));

            var element = parentElement.SelectNodes(elementName);

            if (element == null)
            {
                if (required)
                    throw new ApplicationException("Element must exist: " + elementName);

                return null;
            }

            return element;
        }
        private static XmlElement GetChildElementFromName(XmlNode parentElement, string elementName, bool required)
        {
            if (parentElement == null) throw new ArgumentNullException(nameof(parentElement));
            if (elementName == null) throw new ArgumentNullException(nameof(elementName));

            var element = parentElement.SelectSingleNode(elementName);

            if (element == null)
            {
                if (required)
                    throw new ApplicationException("Element must exist: " + elementName);

                return null;
            }

            return (XmlElement)element;
        }

        private static string GetChildElementInnerText(XmlNode parentElement, string elementName, bool required)
        {
            var element = GetChildElementFromName(parentElement, elementName, required);

            if (element == null)
                return null;

            var innerText = element.InnerText;

            if (string.IsNullOrEmpty(innerText))
            {
                if (required)
                    throw new ApplicationException("Element inner text cannot be empty: " + elementName);
            }

            return innerText;
        }

        private static Uri GetChildElementInnerTextAsUri(XmlNode parentElement, string elementName, bool required)
        {
            var innerText = GetChildElementInnerText(parentElement, elementName, required);

            if (string.IsNullOrEmpty(innerText))
                return null;

            Uri returnValue;
            if (!Uri.TryCreate(innerText, UriKind.Absolute, out returnValue))
                throw new ApplicationException("Value is not a valid Uri:" + innerText);

            return returnValue;
        }

        private static X509Certificate2 GetChildElementCertificateReferenceAsX509Certificate(XmlElement parentElement)
        {
            var thumbprint = GetAttributeValueAsString(parentElement, "thumbprint", true);
            var storelocation = GetAttributeValueAsString(parentElement, "storeLocation", true);
            var storename = GetAttributeValueAsString(parentElement, "storeName", true);
            return LoadCertificate(storename, storelocation, thumbprint);
        }
        private static X509Certificate2 LoadCertificate(string storename, string storelocation, string value)
        {
            try
            {

                var _storeName = (StoreName)Enum.Parse(typeof(StoreName), storename);
                var _storeLocation =
                    (StoreLocation)Enum.Parse(typeof(StoreLocation), storelocation);

                var _store = new X509Store(_storeName, _storeLocation);
                _store.Open(OpenFlags.ReadOnly);
                return _store.Certificates.Find(X509FindType.FindByThumbprint, value, true)[0];
            }
            catch (Exception ex)
            {
                Logging.Instance.Error(ex, "Cannot load client certificate.");
                return null;
            }
        }
        private static X509Certificate2 GetChildElementInnerTextAsX509Certificate(XmlNode parentElement, string elementName, bool required)
        {
            var innerText = GetChildElementInnerText(parentElement, elementName, required);

            if (string.IsNullOrEmpty(innerText))
                return null;

            var bytes = Convert.FromBase64String(innerText);

            var certificate = new X509Certificate2(bytes);

            return certificate;
        }

        private static EndpointAddress CreateEndpointAddress(Uri serviceUrl, string dnsName)
        {
            return new EndpointAddress(serviceUrl, EndpointIdentity.CreateDnsIdentity(dnsName));
        }

        private static string GetAttributeValueAsString(XmlElement element, string attributeName, bool required)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (attributeName == null) throw new ArgumentNullException(nameof(attributeName));

            var attributeValue = element.GetAttribute(attributeName);

            if (string.IsNullOrEmpty(attributeValue))
            {
                if (required)
                    throw new ApplicationException("Attribute is missing on element: " + element.Name + ". Attribute: " + attributeName);
            }

            return attributeValue;
        }
        #endregion
    }
}
