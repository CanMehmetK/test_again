using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Linq;


namespace Ideative.Mvc
{
    public class ViewPathProvider : VirtualPathProvider
    {
        public override bool FileExists(string virtualPath)
        {
            return Pages.IsExistByVirtualPath(virtualPath) || base.FileExists(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (Pages.IsExistByVirtualPath(virtualPath))
            {
                return new ViewFile(virtualPath);
            }

            return base.GetFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, System.Collections.IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (Pages.IsExistByVirtualPath(virtualPath))
                return ViewCacheDependencyManager.Instance.Get(virtualPath);
            
            return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }
    }
}