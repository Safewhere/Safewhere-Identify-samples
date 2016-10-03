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

namespace Safewhere.samples.STS.WebsiteDemo
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

        public void LoginAction(AbstractEndpointHandler handler, HttpContext context,
            dk.nita.saml20.Saml20Assertion assertion)
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
            //var idpEndpoint = handler.GetEndpoint((string)context.Session[AbstractEndpointHandler.IdpTempSessionKey]);
            //Logging.Instance.Information("SamlPricipalAction - LogOnAction: idpEndpoint Id = {0}, Name = {1} OmitAssertionSignatureCheck = {2}", idpEndpoint.Id, idpEndpoint.Name, idpEndpoint.OmitAssertionSignatureCheck);

            Saml2SecurityTokenHandler securityTokenHandler =
                this.sessionAuthModule.FederationConfiguration.IdentityConfiguration.SecurityTokenHandlers[
                    typeof (Saml2SecurityToken)] as Saml2SecurityTokenHandler;

            //Generate bootstraptoken from assertion xml
            if (securityTokenHandler != null)
            {
                var assertionXml = assertion.XmlAssertion;
                System.IO.StringReader reader = new System.IO.StringReader(assertionXml.OuterXml);
                XmlReader xReader = XmlReader.Create(reader);
                var bootstraptoken = securityTokenHandler.ReadToken(xReader);
                HttpContext.Current.Session["boostraptoken"] = bootstraptoken;
            }

            //var nameClaimType = SamlPrincipalAction.DetermineNameClaimType(idpEndpoint, securityTokenHandler);
            //var roleClaimType = SamlPrincipalAction.DetermineRoleClaimType(idpEndpoint, securityTokenHandler);
            //Logging.Instance.Information("SamlPricipalAction - LogOnAction: nameClaimType = {0}, roleClaimType = {1}", nameClaimType, roleClaimType);

            //DateTime validTo;
            //if (assertion.Conditions != null && assertion.Conditions.NotOnOrAfter.HasValue)
            //{
            //    Logging.Instance.Information("Creating CreateSessionSecurityToken. Assertion.Conditions.NotOnOrAfter exists. Use it for validTo.");
            //    validTo = assertion.Conditions.NotOnOrAfter.Value;
            //}
            //else
            //{
            //    Logging.Instance.Information("Creating CreateSessionSecurityToken. Either Assertion.Conditions is null or its NotOnOrAfter doesn't exist. Use UtcNow + SessionSecurityTokenHandler.DefaultTokenLifetime for validTo.");
            //    validTo = DateTime.UtcNow + SessionSecurityTokenHandler.DefaultTokenLifetime;
            //}

            var principal = BuildClaimsPrincipal(assertion);
            
            //var sessionSecurityToken = this.sessionAuthModule.CreateSessionSecurityToken(principal, string.Empty, DateTime.UtcNow, validTo, false);
            //this.sessionAuthModule.AuthenticateSessionSecurityToken(sessionSecurityToken, true);

            //SaveNameId(context, assertion);
        }

        private ClaimsPrincipal BuildClaimsPrincipal(dk.nita.saml20.Saml20Assertion assertion)
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
            //ClaimsPrincipal.Current.AddIdentity(claimsIdentity);
            if (string.IsNullOrEmpty(claimsPrincipal.Identity.Name))
            {
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "No value set for NameClaimType: '{0}'", claimsIdentity.NameClaimType));
            }

            return claimsPrincipal;
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

        private static void SaveNameId(HttpContext context, dk.nita.saml20.Saml20Assertion assertion)
        {
            if (assertion.Subject != null &&
                !string.IsNullOrEmpty(assertion.Subject.Value))
            {
                string nameId = assertion.Subject.Value;
                Logging.Instance.Information("SamlPrincipalAction - SaveNameId nameId = {0}", nameId);
                context.Session["http://www.oasis-open.org/committees/security"] = nameId;
            }
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