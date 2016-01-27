using Ideative.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Ideative.Web
{
    public static class AsseblyLocator
    {

        static Dictionary<string, Assembly> assemblies;
        public static void Init()
        {
            assemblies = new Dictionary<string, Assembly>();
            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
        }

        private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            var t = "Hödööööö";
        }
    }

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AsseblyLocator.Init();
            string strCode = @"    public string ad { get; set; }";
            Ideative.Dinamik.CodeCompiler.Namespace = "Ideative.Web.Models";
            Ideative.Dinamik.CodeCompiler.DebugMode = false;
            Assembly a = Ideative.Dinamik.CodeCompiler.CodeActionsInvoker("Model1", strCode);
            AppDomain.CurrentDomain.Load(a.GetName().Name);

            AreaRegistration.RegisterAllAreas();

            HostingEnvironment.RegisterVirtualPathProvider(new ViewPathProvider());

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // ControllerBuilder.Current.SetControllerFactory(new MyControllerFActory());


        }
    }


    // Test Controller Factory
    public class MyControllerFActory : DefaultControllerFactory
    {
        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            return base.CreateController(requestContext, controllerName);
        }
        public override void ReleaseController(IController controller)
        {
            base.ReleaseController(controller);
        }

    }

}
