using System;
using System.Configuration;
using System.Web;
using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Safewhere.Samples.STS.ClaimAppService
{
    public class Global : HttpApplication
    {
        private IWindsorContainer _container;

        protected void Application_Start(object sender, EventArgs e)
        {
            _container = new WindsorContainer();
            _container.AddFacility<WcfFacility>();
            _container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_container.Kernel));
            _container.Register(Component.For<IService>()
                .ImplementedBy<Service>()
                .LifestyleTransient());
            _container.Install(new ServiceInstaller());
            _container.Register(Component.For<IWindsorContainer>()
                .Instance(_container)
                .LifestyleSingleton());
        }


        private static string ReadConfig(string key, string defaultValue = "")
        {
            var s = ConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(s) ? defaultValue : s;
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            var error = Server.GetLastError();
            //EventLogger.Current.LogError("Unhandled exception", error);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (_container == null)
                return;
            _container.Dispose();
        }
    }

    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyContaining<Service>()
                .Where(t => t.Name.EndsWith("Service", StringComparison.CurrentCulture))
                .WithServiceAllInterfaces()
                .LifestyleTransient());

        }
    }
}