#region

using System;
using System.Configuration;
using System.Web.UI;
using dk.nita.saml20.config;

#endregion

namespace Safewhere.samples.STS.WebsiteDemo
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "This class is derived from OIOSAML.Net 2.0 and thus we would like to leave this as-is.")]
    public partial class Default : Page
    {
        /// <summary>
        ///     Indicates whether the certificate is incorrectly configured in web.config.
        /// </summary>
        protected bool certificateMissing;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                FederationConfig.GetConfig().SigningCertificate.GetCertificate();
                certificateMissing = false;
                // GetCertificate() throws an exception when the certificate can not be retrieved.
            }
            catch (ConfigurationErrorsException)
            {
                certificateMissing = true;
            }
        }
    }
}