#region

using System;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using dk.nita.saml20.config;
using dk.nita.saml20.Logging;
using dk.nita.saml20.protocol;
using Safewhere.Samples.STS.Common;
using Safewhere.Samples.STS.Common.ClaimAppService;

#endregion

namespace Safewhere.samples.STS.WebsiteDemo
{
    public partial class WebForm1 : Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Web.UI.Page.set_Title(System.String)", Justification = "This is a demo project and thus localization is not necessary.")]
        protected void Page_Load(object sender, EventArgs e)
        {
            Title = "My page on SP " + SAML20FederationConfig.GetConfig().ServiceProvider.ID;

            if (Request.QueryString["action"] == "sso")
            {
                // Example of logging required by the requirements BSA6/SSO6 ("Id of internal account that is matched to SAML Assertion")
                // Since FormsAuthentication is used in this sample, the user name to log can be found in context.User.Identity.Name.
                // This user will not be set until after a new redirect, so unfortunately we cannot just log it in our LogAction.LoginAction
                AuditLogging.logEntry(Direction.IN, Operation.LOGIN, "ServiceProvider login",
                    "SP internal user id: " +
                    (Context.User.Identity.IsAuthenticated ? Context.User.Identity.Name : "(not logged in)"));
            }
        }

        protected void Btn_Ping_Click(object sender, EventArgs e)
        {
            var bootstrapToken = HttpContext.Current.Session["boostraptoken"] as SecurityToken;

            if (bootstrapToken == null)
                throw new Exception("Cannot get bootstrap context");

            RequestSecurityTokenConfiguration rstConfig = RequestSecurityTokenConfiguration.Get();

            //Send request to issue security token from Samples.STS
            var token = IdentifyStsServiceCommon.GetSecurityToken(rstConfig, bootstrapToken);

            //Use the issued token to execute the server on Samples.Service
            var service = IdentifyStsServiceCommon.GetClaimAppService<IService>(token, rstConfig.AppliesTo);
            var claims = service.GetClaims();
            var actors = service.GetActorClaims();

            var claimappserviceresonse = new StringBuilder();
            claimappserviceresonse.AppendLine("<table>");
            claimappserviceresonse.AppendFormat(string.Format("<tr><td>There are '{0}' claims on security token.</td></tr>", claims.Length));
            claimappserviceresonse.AppendLine();
            if (claims.Length > 0)
            {
                int i = 1;
                foreach (var claim in claims)
                {
                    claimappserviceresonse.AppendFormat("<tr><td>Claim '{0}': '{1}' - '{2}'</td></tr>", i, claim.ClaimType, claim.Value);
                    claimappserviceresonse.AppendLine();
                    i++;
                }
            }
            claimappserviceresonse.AppendLine("</table>");

            claimappserviceresonse.AppendLine("<table>");
            claimappserviceresonse.AppendFormat(string.Format("<tr><td>There are '{0}' claims on security token's actor.</td></tr>", actors.Length));
            claimappserviceresonse.AppendLine();
            if (actors.Length > 0)
            {
                int i = 1;
                foreach (var claim in actors)
                {
                    claimappserviceresonse.AppendFormat("<tr><td>Claim '{0}': '{1}' - '{2}'</td></tr>", i, claim.ClaimType, claim.Value);
                    claimappserviceresonse.AppendLine();
                    i++;
                }
            }
            claimappserviceresonse.AppendLine("</table>");

            ClaimAppServiceResponse.InnerHtml =
                    string.Format("<p>Successfully ping to Identify service </p><br/><p>{0}</p>", claimappserviceresonse);
        }
        protected void Btn_Relogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("/login.ashx?" + Saml20SignonHandler.IDPForceAuthn + "=true&ReturnUrl=" +
                              HttpContext.Current.Request.Url.AbsolutePath);
        }

        protected void Btn_Passive_Click(object sender, EventArgs e)
        {
            Response.Redirect("/login.ashx?" + Saml20SignonHandler.IDPIsPassive + "=true&ReturnUrl=" +
                              HttpContext.Current.Request.Url.AbsolutePath);
        }

        protected void Btn_ReloginNoForceAuthn_Click(object sender, EventArgs e)
        {
            Response.Redirect("/login.ashx?ReturnUrl=" + HttpContext.Current.Request.Url.AbsolutePath);
        }

        protected void Btn_Logoff_Click(object sender, EventArgs e)
        {
            // Example of logging required by the requirements SLO1 ("Id of internal account that is matched to SAML Assertion")
            // Since FormsAuthentication is used in this sample, the user name to log can be found in context.User.Identity.Name
            AuditLogging.logEntry(Direction.OUT, Operation.LOGOUTREQUEST,
                "ServiceProvider logoff requested, local user id: " + HttpContext.Current.User.Identity.Name);
            Response.Redirect("logout.ashx");
        }
    }
}