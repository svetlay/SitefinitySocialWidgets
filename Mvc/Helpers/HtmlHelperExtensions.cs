using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Pages.Model;
using Telerik.Sitefinity.Web;
using Telerik.Sitefinity.Configuration;

namespace SitefinityWebApp.Mvc.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static string SitefinityActionLink(this HtmlHelper helper, string actionName, string pageId = null)
        {
            return GetSitefinityActionUrl(pageId, actionName);
        }
  
        public static string GetSitefinityActionUrl(string pageId, string actionName)
        {
            PageNode pNode;
            PageManager manager = PageManager.GetManager();
            if (pageId == null)
            {
                pNode = manager.GetPageNode(new Guid(SiteMapBase.GetCurrentProvider().CurrentNode.Key));
            }
            else
            {
                pNode = manager.GetPageNode(new Guid(pageId));
            }
               
            Uri url = HttpContext.Current.Request.Url;
            

            return String.Format("{0}{1}/{2}", ExtractBaseUrl().TrimEnd('/'), pNode.GetFullUrl().TrimStart('~'), actionName);
        }

        //.NET: Y U NO HAVE METHOD FOR THIS
        private static string ExtractBaseUrl()
        {

              return UrlPath.ResolveAbsoluteUrl("~/");
        }
    }
}