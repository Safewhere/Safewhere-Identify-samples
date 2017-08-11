#region

using System;
using System.Diagnostics.CodeAnalysis;
using dk.nita.saml20.config;

#endregion

namespace Safewhere.Samples.STS.WebsiteDemo
{
    public partial class IDPSelectionDemo : System.Web.UI.Page
    {
        [SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Web.UI.WebControls.Literal.set_Text(System.String)", Scope = "member", Target = "Safewhere.Samples.STS.ClaimAppWeb.IDPSelectionDemo.#Page_Load(System.Object,System.EventArgs)", Justification = "This is a demo project and thus localization is not necessary.")]
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "ul")]
        protected void Page_Load(object sender, EventArgs e)
        {
            litIDPList.Text = "<ul>";
            SAML20FederationConfig.GetConfig().Endpoints.IDPEndPoints.ForEach(idp =>
                litIDPList.Text +=
                    "<li><a href=\"" + idp.GetIDPLoginUrl(false, false) + "\">" +
                    (string.IsNullOrEmpty(idp.Name) ? idp.Id : idp.Name) + "</a></li>");
            litIDPList.Text += "</ul>";
        }
    }
}