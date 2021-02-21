using Elevate.IOC;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace Elevate
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers
            IOCRegistration.Register(container);
            AutoMapper.SetupMappingsAndRegisterWithIOC(container);

            // e.g. container.RegisterType<ITestService, TestService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}