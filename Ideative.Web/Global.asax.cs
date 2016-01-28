using Ideative.Mvc;
using ServiceLocation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            
            //AppDomain.CurrentDomain.GetAssemblies()[85].GetName().Name
            //if(args.LoadedAssembly.GetName().)
            Debug.WriteLine(args.LoadedAssembly.GetName());
            Debug.WriteLine(sender.ToString());
            AppDomain.CurrentDomain.Load(args.LoadedAssembly.GetName());
            var assbly = AppDomain.CurrentDomain.GetAssemblies().Where(ass => ass.GetName().Name == args.LoadedAssembly.GetName().Name);
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
            //AppDomain.CurrentDomain.Load(a.GetName().Name);
            Debug.WriteLine(" Name : " + a.GetName().Name);
            Debug.WriteLine(" NamF : " + a.GetName());
            Debug.WriteLine(" NamF : " + a.FullName);

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


    #region Dynamic Controller / Factory
    //public class DynamicController : Controller
    //{

    //}
    //public interface IFindAction
    //{
    //    ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor, string actionName);
    //}
    //public class DynamicControllerFactory : DefaultControllerFactory
    //{
    //    private readonly IServiceLocator _Locator;
    //    public DynamicControllerFactory(IServiceLocator locator) { _Locator = locator; }
    //    protected override Type GetControllerType(RequestContext requestContext, string controllerName)
    //    {
    //        var controllerType = base.GetControllerType(requestContext, controllerName);
    //        return controllerType ?? typeof(DynamicController);
    //    }

    //    protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
    //    {
    //        var controller = base.GetControllerInstance(requestContext, controllerType) as Controller;
    //        var actionInvoker = _Locator.GetInstance<IActionInvoker>();
    //        if (actionInvoker != null) { controller.ActionInvoker = actionInvoker; }
    //        return controller;
    //    }
    //}

    //public class DynamicActionInvoker : ControllerActionInvoker
    //{
    //    private readonly IServiceLocator _Locator;
    //    public DynamicActionInvoker(IServiceLocator locator) { _Locator = locator; }

    //    protected override ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor, string actionName)
    //    {
    //        // try to match an existing action name first
    //        var action = base.FindAction(controllerContext, controllerDescriptor, actionName);
    //        if (action != null)
    //        {
    //            return action;
    //        }

    //        // @ray247 The remainder of this you'd probably write on your own...
    //        var actionFinders = _Locator.GetAllInstances<IFindAction>();
    //        if (actionFinders == null)
    //        {
    //            return null;
    //        }

    //        return actionFinders
    //            .Select(f => f.FindAction(controllerContext, controllerDescriptor, actionName))
    //            .Where(d => d != null)
    //            .FirstOrDefault();
    //    }
    //} 
    #endregion

}
