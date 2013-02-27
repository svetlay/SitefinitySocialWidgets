using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SitefinityWebApp.Mvc.Configuration;
using Telerik.Sitefinity.Configuration;

namespace SitefinityWebApp.Mvc.Helpers
{
    public class SocialMediaConnectConstants
    {


        public static FacebookConfig FbConfig
        {
            get
            {
                return Config.Get<FacebookConfig>();
            }
        }
        /// <summary>
        /// Getter for the AppId Retrieved from developer.facebook.com
        /// </summary>
        public static string AppId
        {
            get
            {

                return FbConfig.AppId;
            }
        }

        public static string SiteBaseUrl
        {
            get
            {
                return FbConfig.SiteBaseUrl;
            }
        }


        /// <summary>
        /// Your private key or secret retrieved from developer.facebook.com
        /// </summary>
        public static string AppSecret
        {
            get
            {

                return FbConfig.AppSecret;
            }
        }

    }
}