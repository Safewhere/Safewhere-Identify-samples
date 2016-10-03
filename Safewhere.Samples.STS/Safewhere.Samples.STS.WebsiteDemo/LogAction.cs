#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Web;
using System.Xml;
using dk.nita.saml20;
using dk.nita.saml20.Actions;
using dk.nita.saml20.identity;
using dk.nita.saml20.Logging;
using dk.nita.saml20.protocol;
using Safewhere.Samples.STS.Common;

#endregion

namespace Safewhere.Samples.STS.WebsiteDemo
{
    public class LogAction : IAction
    {
        private SessionAuthenticationModule sessionAuthModule;

        #region Constructors

        public LogAction()
            : this(FederatedAuthentication.SessionAuthenticationModule)
        { }

        public LogAction(SessionAuthenticationModule sessionAuthModule)
        {
            if (sessionAuthModule == null)
            {
                throw new ArgumentNullException("sessionAuthModule");
            }

            this.sessionAuthModule = sessionAuthModule;
        }

        #endregion

        #region IAction Members

        public void LoginAction(AbstractEndpointHandler handler, HttpContext context, Saml20Assertion assertion)
        {
            if (handler == null)
            {
                Logging.Instance.Error("SamlPrincipalAction - LogOnAction handler is null");
                throw new ArgumentNullException("handler");
            }
            if (context == null)
            {
                Logging.Instance.Error("SamlPrincipalAction - LogOnAction context is null");
                throw new ArgumentNullException("context");
            }
            if (assertion == null)
            {
                Logging.Instance.Error("SamlPrincipalAction - LogOnAction assertion is null");
                throw new ArgumentNullException("assertion");
            }
            Saml2SecurityTokenHandler securityTokenHandler =
                this.sessionAuthModule.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers[
                    typeof (Saml2SecurityToken)] as Saml2SecurityTokenHandler;

            //Generate bootstraptoken from assertion xml
            if (securityTokenHandler != null)
            {
                var assertionXml = assertion.XmlAssertion;
                using (System.IO.StringReader reader = new System.IO.StringReader(assertionXml.OuterXml))
                {
                    XmlReader xReader = XmlReader.Create(reader);
                    var bootstraptoken = securityTokenHandler.ReadToken(xReader);
                    HttpContext.Current.Session["boostraptoken"] = bootstraptoken;
                }
            }
            BuildClaimsPrincipal(assertion);
        }

        private void BuildClaimsPrincipal(Saml20Assertion assertion)
        {
            Saml2Assertion bootstrapAssertion = CreateSaml2Assertion(assertion); 

            var claimsPrincipal = new ClaimsPrincipal();
            var claimsIdentity = new ClaimsIdentity("Saml2", ClaimTypes.NameIdentifier, ClaimTypes.Role)
            {
                BootstrapContext =
                    new BootstrapContext(new Saml2SecurityToken(bootstrapAssertion), new Saml2SecurityTokenHandler())
            };


            // Populate the claims in the IClaimsIdentity with the attributes in the Saml2Assertion
            foreach (var samlAttribute in assertion.Attributes)
            {
                Claim claim = new Claim(
                                   samlAttribute.Name,
                                   samlAttribute.AttributeValue[0]);
                claimsIdentity.AddClaim(claim);
            }
            if (string.IsNullOrEmpty(claimsIdentity.Name))
            {
                if (assertion.Subject != null && !string.IsNullOrEmpty(assertion.Subject.Value))
                {
                    claimsIdentity.AddClaim(new Claim(claimsIdentity.NameClaimType, assertion.Subject.Value));
                }
            }
            claimsPrincipal.AddIdentity(claimsIdentity);
            if (string.IsNullOrEmpty(claimsPrincipal.Identity.Name))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "No value set for NameClaimType: '{0}'", claimsIdentity.NameClaimType));
            }

            return;
        }

        private Saml2Assertion CreateSaml2Assertion(Saml20Assertion assertion)
        {
            var result = new Saml2Assertion(new Saml2NameIdentifier(assertion.Subject.Value));
            var attributes = new List<Saml2Attribute>();
            foreach (var samlAttribute in assertion.Attributes)
            {
                attributes.Add(new Saml2Attribute(samlAttribute.Name, samlAttribute.AttributeValue));
            }
            result.Statements.Add(new Saml2AttributeStatement(attributes));
            return result;
        }

        public void LogoutAction(AbstractEndpointHandler handler, HttpContext context,
            bool IdPInitiated)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            // Example of logging required by the requirements SLO1 ("Id of internal user account")
            // Since FormsAuthentication is used in this sample, the user name to log can be found in context.User.Identity.Name
            // The login will be not be cleared until next redirect due to the way FormsAuthentication works, so we will have to check Saml20Identity.IsInitialized() too
            AuditLogging.logEntry(Direction.IN, Operation.LOGOUT, "ServiceProvider logout",
                "SP local user id: " + (context.User.Identity.IsAuthenticated ? context.User.Identity.Name : "none") +
                " login status: " + Saml20Identity.IsInitialized());
        }

        /// <summary>
        ///     <see cref="IAction.SoapLogoutAction" />
        /// </summary>
        public void SoapLogoutAction(AbstractEndpointHandler handler, HttpContext context, string userId)
        {
            AuditLogging.logEntry(Direction.IN, Operation.LOGOUT, "ServiceProvider SOAP logout",
                "IdP user id: " + userId + " login status: " + Saml20Identity.IsInitialized());
        }

        public string Name
        {
            get { return "LogAction"; }
            set { }
        }

        #endregion
    }
}