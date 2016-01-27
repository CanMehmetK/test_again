using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ideative.Web.Controllers
{
    public class DynamicModelTestController : Controller
    {
        // GET: DynamicModelTest
        public ActionResult Index()
        {
            var type = Type.GetType("Ideative.Web.Models.Model1");
            var model = Activator.CreateInstance(type);
            var property = type.GetProperty("ad");
            property.SetValue(model, "adım bu");
            return View();
        }
    }
}