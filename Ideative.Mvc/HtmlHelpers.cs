using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System.Web.Mvc
{
    public static partial class IdeativeHtmlHelpers
    {
        public static MvcHtmlString DynamicEditorFor(this HtmlHelper htmlHelper,
                   dynamic instance,string ad)
        {
            var tType = instance.GetType();
            var prop = (instance.GetType()).GetProperty(ad);
            
            return new MvcHtmlString(@"<input type=""text"" value='"+prop.GetValue(instance)+"'></input>");
        }
    }
}
