using System;
using System.Web;

namespace Safewhere.Samples.SameSiteHttpModule
{
    /// <summary>
    /// Credit: https://charliedigital.com/2020/01/22/adventures-in-single-sign-on-samesite-doomsday/
    /// </summary>
    public class SameSiteHttpModule : IHttpModule
    {
        private const SameSiteMode Unspecified = (SameSiteMode)(-1);

        /// <summary>
        ///     Set up the event handlers.
        /// </summary>
        public void Init(HttpApplication context)
        {
            // This one is the OUTBOUND side; we add the extra cookie
            context.PreSendRequestHeaders += OnEndRequest;
        }

        /// <summary>
        ///     The OUTBOUND LEG; we modify the cookies.
        /// </summary>
        private void OnEndRequest(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;

            HttpContext context = application.Context;

            SameSiteMode cookieSameSite = UserAgentDetectionLib.DisallowsSameSiteNone(context.Request.UserAgent) ? Unspecified : SameSiteMode.None;

            for (int i = 0; i < context.Response.Cookies.Count; i++)
            {
                // Change all cookies to either None or leave it empty depends on browser version 
                HttpCookie responseCookie = context.Response.Cookies[i];

                if (responseCookie.SameSite == cookieSameSite)
                    continue;

                responseCookie.SameSite = cookieSameSite;
            }
        }

        public void Dispose()
        {

        }
    }
}
