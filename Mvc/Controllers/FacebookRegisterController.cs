using System;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;
using Facebook;
using SitefinityWebApp.Mvc.Helpers;
using SitefinityWebApp.Mvc.Models;
using Telerik.Sitefinity.Security.Claims;
using SitefinityWebApp.Mvc.Helpers;

namespace SitefinityWebApp.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "FacebookRegister", Title = "FacebookRegister", SectionName = "MvcWidgets")]
    public class FacebookRegisterController : Controller
    {
        public string RedirectUrl
        {
            get
            {

                return HtmlHelperExtensions.GetSitefinityActionUrl(null, "SubmitFacebookRegistration");
            }
        }
        
        /// <summary>
        /// This is the default Action.
        /// </summary>
        ///
        [HttpGet]
        public ActionResult Index()
        {
            Guid currentUserId = ClaimsManager.GetCurrentUserId();
            if (currentUserId == Guid.Empty)
            {
                
              
                return View("Connect");
            }
            else
            {
                var model = new SitefinityUserModel();
                return View("ViewProfile", model);
            }
        }
        
        [HttpGet]
        public ActionResult AuthenticateFacebook()
        {
            // Build the Return URI form the Request Url
            var redirectUri = new UriBuilder(Request.Url);
            redirectUri.Path = RedirectUrl;

            var client = new FacebookClient();

            // Generate the Facebook OAuth URL
            // Example: https://www.facebook.com/dialog/oauth?
            //                client_id=YOUR_APP_ID
            //               &redirect_uri=YOUR_REDIRECT_URI
            //               &scope=COMMA_SEPARATED_LIST_OF_PERMISSION_NAMES
            //               &state=SOME_ARBITRARY_BUT_UNIQUE_STRING
            var uri = client.GetLoginUrl(new
            {
                client_id = SocialMediaConnectConstants.AppId,
                redirect_uri = RedirectUrl,
                scope = "email"
                ,
            });

            return Redirect(uri.ToString());
        }

        public ActionResult SubmitFacebookRegistration()
        {
            var model = FacebookAuthenticationHelper.FacebookHandshake(RedirectUrl, Request);
            SocialMediaConnectStatus status = FacebookAuthenticationHelper.Register(model, "Default");

            if (status == SocialMediaConnectStatus.Registered || status == SocialMediaConnectStatus.LoggedIn)
                return Redirect("/");

            else
            {
                //TODO: Replace with comprehensive error view;
                return Content("Operation Failed");
            }
        }
    }
}