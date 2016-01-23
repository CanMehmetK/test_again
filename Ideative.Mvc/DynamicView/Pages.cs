using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace Ideative.Mvc
{
    public class Pages
    {
        // TODO@kanpinar binary de olabilir.
        public static bool IsExistByVirtualPath(string virtualPath)
        {
            return false;

            //TODO@kanpinar: veritabanında view adına bak. varsa evet döndür.

            if (virtualPath.StartsWith("~/"))
                virtualPath = virtualPath.Substring(1);

            #region Check From Assebly
            var assembly = Assembly.LoadFrom(HttpContext.Current.Server.MapPath("~/bin") + "\\Falafel.Resources.dll");
            string result = string.Empty;
            virtualPath = "Falafel.Resources" + virtualPath.Replace('/', '.');

            if (virtualPath.EndsWith("/"))
            {
                result = assembly.GetManifestResourceNames().First();
            }
            else
            {
                result = assembly.GetManifestResourceNames().FirstOrDefault(i => i.ToLower() == virtualPath.ToLower());
            } 
            #endregion

            return string.IsNullOrEmpty(result) ? false : true;
        }

        public static string GetByVirtualPath(string virtualPath)
        {
            if (virtualPath.StartsWith("~/"))
                virtualPath = virtualPath.Substring(1);

            #region Read From Assebly
            var assembly = Assembly.LoadFrom(HttpContext.Current.Server.MapPath("~/bin") + "\\Falafel.Resources.dll");
            virtualPath = "Falafel.Resources" + virtualPath.Replace('/', '.');
            virtualPath = assembly.GetManifestResourceNames().FirstOrDefault(i => i.ToLower() == virtualPath.ToLower());

            using (Stream stream = assembly.GetManifestResourceStream(virtualPath))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    return result;
                }
            } 
            #endregion

        }
    }
}