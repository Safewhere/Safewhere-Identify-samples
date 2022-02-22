using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Mvc;

namespace Safewhere.External.Interceptors
{
    /// <summary>
    /// This interface defines methods which are used to intercept login flows on the authentication side.
    /// </summary>
    /// <remarks>
    /// A typical login flow in Identify is:
    /// 1. A user accesses a service provider and is redirected to Identify. Identify then redirects the user to an authentication provider where he or she will enter credentials to login.
    /// 
    /// 2. The provider returns a token to Identify. Identify validates the token and creates a local authentication session (*)
    /// 
    /// 3. The token is then passed (**) to claims transformation pipeline and returned to the service provider.
    /// Please note that (*) happens on the authentication side while (**) happens on the protocol side.
    /// 
    /// Some example use cases about what an interceptor may want to do are:
    /// - Check if a token meets some custom conditions. For example, a login user must belong to some specific organizations.
    /// - Display a UI to ask a user for more information.
    /// - Use the token to look up for matched accounts (aka profiles) in the Identify's local user store or in an AD. If there are multiple matches, display a UI which lists all the profiles so that the user can pick one.
    /// </remarks>
    public interface IAuthenticationInterceptorService
    {
        /// <summary>
        /// Intercepts an login flow on the authentication side.
        /// </summary>
        /// <param name="cc">The controllercontext object of the current request</param>
        /// <param name="principal">A claims principal which Identify receives from an upstream Identity Provider</param>
        /// <param name="requestInformation">An object of type <see cref="Safewhere.External.Model.IIdentifyRequestInformation"/></param>
        /// <param name="input">Static settings which are configured on the authentication connection UI. Each specific implementation requires different static settings.
        /// For example, an interceptor which needs to access a database will need a connection string, while one which needs to access an AD store will need to know where the AD server is</param>
        /// <param name="contextId">Each login a user does with Identify will have a context id.</param>
        /// <param name="viewName">Name of the main view which an interceptor may want to display to a user.
        /// Interceptors which want to have more than one configurable views can use the input parameter instead.</param>
        /// <returns>A null value means that this interceptor has done its job and doesn't want to intercept the login flow.</returns>
        /// <remarks>
        /// When an authentication or a protocol connection is configured to use an interceptor, Identify will call this method of the interceptor.
        /// This is the chance for the interceptor to do whatever it wants to do before Identify takes control back and proceeds to the next step.
        /// </remarks>
        ActionResult Intercept(ControllerContext cc, ClaimsPrincipal principal, IIdentifyRequestInformation requestInformation, IDictionary<string, string> input,
                               string contextId, string viewName);

        /// <summary>
        /// In the event of the login flow was intercepted and a UI is shown to a user, this method is called to handle data which the user submits.
        /// </summary>
        /// <param name="cc">The controllercontext object of the current request</param>
        /// <param name="principal">A claims principal which Identify receives from an upstream Identity Provider</param>
        /// <param name="requestInformation">An object of type <see cref="Safewhere.External.Model.IIdentifyRequestInformation"/></param>
        /// <param name="input">Static settings which are configured on the authentication connection UI. Each specific implementation requires different static settings.
        /// For example, an interceptor which needs to access a database will need a connection string, while one which needs to access an AD store will need to know where the AD server is</param>
        /// <param name="contextId">Each login a user does with Identify will have a context id.</param>
        /// <param name="viewName">Name of the main view which an interceptor may want to display to a user.
        /// Interceptors which want to have more than one configurable views can use the input parameter instead.</param>
        /// <returns>A null value means that this interceptor has done whatever it wants to based on data submitted from the user and the login flow should proceed.
        /// Otherwise, return an ActionResult to continue to intercept the login flow.</returns>
        /// <remarks>
        /// </remarks>
        ActionResult OnPostBack(ControllerContext cc, ClaimsPrincipal principal, IIdentifyRequestInformation requestInformation, IDictionary<string, string> input,
                               string contextId, string viewName);

        /// <summary>
        /// Defines a list of must-have static settings which system administrator must be configured for this interceptor on the authentication connection UI.
        /// </summary>
        /// <remarks>
        /// This attribute will be supported in a near future release.
        /// </remarks>
        IEnumerable<string> MustHaveInputKeys { get; }
    }
}