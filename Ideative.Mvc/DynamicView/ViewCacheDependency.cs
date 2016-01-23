using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Ideative.Mvc
{
    public class ViewCacheDependency : CacheDependency
    {
        public ViewCacheDependency(string virtualPath)
        {
            base.SetUtcLastModified(DateTime.UtcNow);
        }

        public void Invalidate()
        {
            base.NotifyDependencyChanged(this, EventArgs.Empty);
        }
    }
}