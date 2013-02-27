using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Security;
using Facebook;
using SitefinityWebApp.Mvc.Models;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Security.Model;

namespace SitefinityWebApp.Mvc.Helpers
{
    public static class FacebookAuthenticationHelper
    {
        public static SocialMediaConnectStatus Register(FacebookUserModel model, string userProvider)
        {
            SocialMediaConnectStatus connectStatus = SocialMediaConnectStatus.LoggedIn;

            var userManager = UserManager.GetManager(userProvider);
            userManager.Provider.SuppressSecurityChecks = true;
            
            if (!userManager.UserExists("facebook_user" + model.UserId))
            {
                System.Web.Security.MembershipCreateStatus status;
                var user = userManager.CreateUser("facebook_user" + model.UserId, Encrypt(model.UserId), model.Email,
                    "Question1", "Answer1", true, null, out status);

                userManager.SaveChanges();

                userManager.Provider.SuppressSecurityChecks = false;

                SitefinityUserModel sfUserModel = new SitefinityUserModel();

                //var profileManager = UserProfileManager.GetManager();
                //var profile = profileManager.CreateProfile(user, typeof(SitefinityProfile).FullName) as SitefinityProfile;
                //profile.FirstName = model.FirstName;
                //profile.LastName = model.LastName;

                //TODO: Cast Facebook Model to Sitefinity Model
                sfUserModel.CreatedUserId = user.Id;
                sfUserModel.FirstName = model.FirstName;
                sfUserModel.LastName = model.LastName;
                sfUserModel.Avatar = model.ProfileImageUrl;
                sfUserModel.Birthday = model.Birthday;
                sfUserModel.Location = model.Location;
                sfUserModel.Gender = model.Gender;

                RoleManager roleManager = RoleManager.GetManager();
                roleManager.Provider.SuppressSecurityChecks = true;

                roleManager.Provider.SuppressSecurityChecks = false;

                if (status != MembershipCreateStatus.Success)
                {
                    return SocialMediaConnectStatus.Failed;
                }

                connectStatus = SocialMediaConnectStatus.Registered;
            }

            UserLoggingReason loginStatus = Login(model, "Default");
            Login(model, userProvider);

            return connectStatus;
        }

        public static UserLoggingReason Login(FacebookUserModel model, string userProvider)
        {
            Credentials credentials = new Credentials()
            {
                UserName = "facebook_user" + model.UserId,
                Password = Encrypt(model.UserId),
                Persistent = false,
                MembershipProvider = userProvider
            };

            if (credentials == null)
            {
                var result = UserLoggingReason.Unknown;
                return result;
            }

            return SecurityManager.AuthenticateUser(credentials);
        }

        public static string Encrypt(string plainTextPassword)
        {
             string password = FormsAuthentication.HashPasswordForStoringInConfigFile(plainTextPassword, "SHA1");

             return password;

        }


        public static FacebookUserModel FacebookHandshake(string redirectUri, HttpRequestBase request)
        {
            var model = new FacebookUserModel();
            var client = new FacebookClient();
            var oauthResult = client.ParseOAuthCallbackUrl(request.Url);

            // Build the Return URI form the Request Url

            // Exchange the code for an access token
            dynamic result = client.Get("/oauth/access_token", new
            {
                client_id = SocialMediaConnectConstants.AppId,
                redirect_uri = redirectUri,
                client_secret = SocialMediaConnectConstants.AppSecret,
                code = oauthResult.Code
                ,
            });

            // Read the auth values
            string accessToken = result.access_token;

            //If needed you can add the access token to a cookie for pulling additional inforamtion out of Facebook

            //DateTime expires = DateTime.UtcNow.AddSeconds(Convert.ToDouble(result.expires));

            //HttpCookie myCookie = HttpContext.Current.Request.Cookies["accessToken"] ?? new HttpCookie("accessToken");
            //myCookie.Values["value"] = accessToken;
            //myCookie.Expires = expires;

            //HttpContext.Current.Response.Cookies.Add(myCookie);

            // Get the user's profile information
            dynamic me = client.Get("/me",
                new
            {
                fields = "name,picture,first_name,last_name,email,id,birthday,location,gender",
                access_token = accessToken
            });

            // Read the Facebook user values
            model.UserId = me.id;
            model.FirstName = me.first_name;
            model.LastName = me.last_name;
            model.Email = me.email;
            model.ProfileImageUrl = ExtractImageUrl(me);
            model.Birthday = me.birthday;
            model.Gender = me.gender;
            model.Location = me.location["name"].ToString();
            return model;
        }
  
        public static string ExtractImageUrl(dynamic me)
        {
            string imageRequestUrl = String.Format("https://graph.facebook.com/{0}/picture?type=large", me.id);
            WebResponse response = null;
            string pictureUrl = string.Empty;

            WebRequest pictureRequest = WebRequest.Create(imageRequestUrl);
            response = pictureRequest.GetResponse();
            pictureUrl = response.ResponseUri.ToString();
            return pictureUrl;
        }

        public static void ExtractCityUrl(dynamic me)
        {
            //TODO: Imlement using Wikipedia API 
        }
    }
}