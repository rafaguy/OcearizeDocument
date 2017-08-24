using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Umbraco.Web.Routing;

namespace OCR.Application
{
    public class CustomContentFinder : IContentFinder
    {
        public bool TryFindContent(PublishedContentRequest contentRequest)
        {
            var path = contentRequest.Uri.AbsolutePath;
            if(path.StartsWith("/woot"))
            {
                return false;
            }
            return true;
        }
    }
}