using System;
using System.Configuration;
using System.Linq;
using Telerik.Sitefinity.Configuration;
using Telerik.Sitefinity.Localization;

namespace SitefinityWebApp.Mvc.Configuration
{
    [ObjectInfo(Title = "FacebookConfig Title", Description = "FacebookConfig Description")]
    public class FacebookConfig : ConfigSection
    {


        [ObjectInfo(Title = "AppId", Description = "Enter your facebook App Id.")]
        [ConfigurationProperty("AppId", DefaultValue = "")]
        public string AppId
        {
            get
            {
                return (string)this["AppId"];
            }
            set
            {
                this["AppId"] = value;
            }
        }


        [ObjectInfo(Title = "AppSecret", Description = "Enter your facebook App Secret.")]
        [ConfigurationProperty("AppSecret", DefaultValue = "")]
        public string AppSecret
        {
            get
            {
                return (string)this["AppSecret"];
            }
            set
            {
                this["AppSecret"] = value;
            }
        }

        [ObjectInfo(Title = "SiteBaseUrl", Description = "The Base Url of your site. Make sure this has been configured with your facebook app")]
        [ConfigurationProperty("SiteBaseUrl", DefaultValue = "")]
        public string SiteBaseUrl
        {
            get
            {
                return (string)this["SiteBaseUrl"];
            }
            set
            {
                this["SiteBaseUrl"] = value;
            }
        }

    }
}