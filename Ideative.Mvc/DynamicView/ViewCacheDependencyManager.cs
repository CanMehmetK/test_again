using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Ideative.Mvc
{
    public class ViewCacheDependencyManager
    {
        private static Dictionary<string, ViewCacheDependency> dependencies = new Dictionary<string, ViewCacheDependency>();
        private static volatile ViewCacheDependencyManager instance;
        private static object syncRoot = new Object();

        private ViewCacheDependencyManager()
        {
        }

        public static ViewCacheDependencyManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new ViewCacheDependencyManager();
                        }
                    }
                }

                return instance;
            }
        }

        public CacheDependency Get(string virtualPath)
        {
            if (!dependencies.ContainsKey(virtualPath))
                dependencies.Add(virtualPath, new ViewCacheDependency(virtualPath));

            return dependencies[virtualPath];
        }

        public void Invalidate(string virtualPath)
        {
            if (dependencies.ContainsKey(virtualPath))
            {
                var dependency = dependencies[virtualPath];
                dependency.Invalidate();
                dependency.Dispose();
                dependencies.Remove(virtualPath);
            }
        }
    }
}