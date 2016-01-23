
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace Ideative.Mvc
{
    public class ViewFile : VirtualFile
    {
        private string path;

        public ViewFile(string virtualPath) : base(virtualPath)
        {
            path = virtualPath;
        }

        public override Stream Open()
        {
            if (string.IsNullOrEmpty(path))
                return new MemoryStream();

            string content = Pages.GetByVirtualPath(path);
            if (string.IsNullOrEmpty(content))
                return new MemoryStream();

            return new MemoryStream(ASCIIEncoding.UTF8.GetBytes(content));
        }
    }
}