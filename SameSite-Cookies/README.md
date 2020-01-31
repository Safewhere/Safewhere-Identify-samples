# SAML 2.0 for WIF component: Add support for the new SameSite cookie behavior for old browsers

Safewhere's SAML 2.0 for WIF component has 3 cookies that are set by Asp.Net/WIF/System.IdentityModel:

- SessionId cookie.
- Forms Authentication cookie.
- FedAuth cookie.

If you doesn't want to support old browsers, you can follow instructions at http://docs.safewhere.com/samesite-cookies/ to set the cookies to None by default.

## Web forms applications

If you do want to support old browsers for your Asp.Net *web forms* applications, the idea is that you can use an IHttpModule to modify the cookies. You can use the Safewhere.Samples.SameSiteHttpModule sample project for testing purpose:

1. Install .NET 4.7.2 or 4.8 on your server as well as your development machine.
2. Compile the Safewhere.Samples.SameSiteHttpModule project.
3. Drop the Safewhere.Samples.SameSiteHttpModule.dll into the bin folder of your application.
4. Add `Safewhere.Samples.SameSiteHttpModule.SameSiteHttpModule` to your application's web.config file:

		```XML
		  <system.webServer>
			<modules>
			  <remove name="ScriptModule"/>
			  <add name="SessionAuthenticationModule" type="System.IdentityModel.Services.SessionAuthenticationModule, System.IdentityModel.Services, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" preCondition="managedHandler" />
			  <add type="Safewhere.Samples.SameSiteHttpModule.SameSiteHttpModule, Safewhere.Samples.SameSiteHttpModule" name="SameSiteHttpModule" preCondition="managedHandler"/>
			</modules>
			<validation validateIntegratedModeConfiguration="false"/>
		  </system.webServer>
		```

## MVC applications

If your SAML 2.0 for WIF-powered applications are using Asp.Net MVC, you can use ActionFilter instead of IHttpModule. Remember to add the SameSiteCookieActionFilter to the GlobalFilter collection.

```CSharp
    public class SameSiteCookieActionFilter : ActionFilterAttribute
    {
        private const SameSiteMode Unspecified = (SameSiteMode)(-1);

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            var cookieSameSite = UserAgentDetectionLib.DisallowsSameSiteNone(httpContext.Request.UserAgent) ? Unspecified : SameSiteMode.None;

            var cookieKeys = httpContext.Response.Cookies.Keys;
            foreach (string cookieKey in cookieKeys)
            {
				// add your logic to 
                HttpCookie cookie = httpContext.Response.Cookies[cookieKey];

                if (cookie.SameSite == cookieSameSite)
                    continue;

                cookie.SameSite = cookieSameSite;
                cookie.Secure = true;
            }
        }
    }
```
