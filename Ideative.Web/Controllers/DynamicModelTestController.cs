using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ideative.Web.Models;

namespace Ideative.Web.Controllers
{
    public class DynamicModelTestController : Controller
    {
        // GET: DynamicModelTest
        public ActionResult Index()
        {

            var type =
                AppDomain.CurrentDomain.GetAssemblies().Where(
                    assx => assx.GetName().Name == "__Ideative.Web")
                    .First()
                    .GetType("Ideative.Web.Models.Model1");

            

            var model = Activator.CreateInstance(type);
            //Name = "Model1"       "Ideative.Web.Models.Model1"}
            var property = type.GetProperty("ad");
            property.SetValue(model, "adım bu");
            return View(model);
        }
    }
}