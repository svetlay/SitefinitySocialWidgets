using System;
using System.Linq;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Security.Claims;
using Telerik.Sitefinity;
using Telerik.Sitefinity.Modules.Libraries;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Telerik.Sitefinity.Workflow;
using System.IO;
using System.Net;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Security.Model;
using Telerik.Sitefinity.Libraries.Model;
using Telerik.Sitefinity.Model.ContentLinks;

namespace SitefinityWebApp.Mvc.Models
{
    public class SitefinityUserModel
    {
        public UserManager Manager
        {
            get
            {
                return UserManager.GetManager();
            }
        }

        public UserProfileManager ProfileManager
        {
            get
            {
                return UserProfileManager.GetManager();
            }
        }

        public SitefinityProfile Profile
        {
            get
            {
                var currentProfile = ProfileManager.GetUserProfile<SitefinityProfile>(Manager.GetUser(UserId));

                if (currentProfile == null)
                {
                    ProfileManager.CreateProfile(Manager.GetUser(UserId), Guid.NewGuid(), typeof(SitefinityProfile));
                    ProfileManager.SaveChanges();
                }
                return ProfileManager.GetUserProfile<SitefinityProfile>(Manager.GetUser(UserId));
            }
        }

        public Guid UserId
        {
            get
            {
                if (ClaimsManager.GetCurrentUserId() == Guid.Empty)
                {
                    return this.CreatedUserId;
                }

                return ClaimsManager.GetCurrentUserId();
            }
        }

        public Guid CreatedUserId { get; set; }

        public string Gender
        {
            get
            {
                var genderCustomField = App.WorkWith().DynamicData().Type(typeof(SitefinityProfile)).Fields().Where(f => f.FieldName == "Gender").Get().SingleOrDefault();

                if (genderCustomField == null)
                {
                    return "";
                }
                else
                {
                    return Profile.GetValue<string>("Gender");
                }
            }

            set
            
            {
                //TODO: Implement Creating the dynamic fields
            
                Profile.SetValue("Gender", value);
                ProfileManager.SaveChanges();
            }
        }

        public string Birthday
        {
            get
            {
                var birthdayCustomField = App.WorkWith().DynamicData().Type(typeof(SitefinityProfile)).Fields().Where(f => f.FieldName == "Birthday").Get().SingleOrDefault();

                if (birthdayCustomField == null)
                {
                    return "";
                }
                else
                {
                    return Profile.GetValue<string>("Birthday");
                }
            }

            set
            {

                Profile.SetValue("Birthday", value);
                ProfileManager.SaveChanges();

            }
        }

        public string Location
        {
            get
            {
                var locationCustomField = App.WorkWith().DynamicData().Type(typeof(SitefinityProfile)).Fields().Where(f => f.FieldName == "Location").Get().SingleOrDefault();

                if (locationCustomField == null)
                {
                    return "";
                }
                else
                {
                    return Profile.GetValue<string>("Location");
                }
            }
            set
            {
                

                Profile.SetValue("Location", value);
                ProfileManager.SaveChanges();
            }
        }

        public string FirstName
        {
            get
            {
                return Profile.FirstName;
            }

            set
            {
                Profile.FirstName = value;
                ProfileManager.SaveChanges();
            }
        }

        public string LastName
        {
            get
            {
                return Profile.LastName;
            }

            set
            {
                Profile.LastName = value;
                ProfileManager.SaveChanges();
            }
        }

        public string Email
        {
            get
            {
                User currentUser = Manager.GetUser(ClaimsManager.GetCurrentUserId());
                return currentUser.Email;
            }
        }

       

        public string Avatar
        {
            get
            {
                LibrariesManager librariesManager = LibrariesManager.GetManager();

                if (Profile.Avatar != null)
                {
                    Telerik.Sitefinity.Libraries.Model.Image image = librariesManager.GetImage(Profile.Avatar.ChildItemId);

                    if (image != null)
                    {
                        return image.MediaUrl;
                    }
                    return "";
                }
                //TODO: return the Sitefinity generic image;
                else
                {
                    return "";
                }
            }
            set
            {
                LibrariesManager librariesManager = LibrariesManager.GetManager();
                librariesManager.Provider.SuppressSecurityChecks = true;
                

                //TODO: Make the Album Configurable via Designer

                Album album = librariesManager.GetAlbums().Where(al => al.Title == "Facebook Users").FirstOrDefault();
                if (album == null)
                {
                    Guid albumId = Guid.NewGuid();
                    CreateAlbumNativeAPI(albumId, "Facebook Users");

                    album = librariesManager.GetAlbum(albumId);
                }
                
                string imageTitle = String.Format("User {0} {1} {2}", FirstName, LastName, new Random().Next(1000));

                string extension = ".jpg";
               
                WebRequest req = HttpWebRequest.Create(value);

                using (Stream stream = req.GetResponse().GetResponseStream())
                {

                    Guid imageId =  CreateImageNativeAPI(imageTitle, stream, extension,librariesManager);

                    Profile.Avatar = new ContentLink(Profile.Id, imageId)
                    {
                        ParentItemProviderName = ProfileManager.Provider.Name,
                        ChildItemProviderName = librariesManager.Provider.Name,
                        ParentItemType = typeof(SitefinityProfile).FullName,
                        ChildItemType = typeof(Image).FullName
                    };

                    ProfileManager.SaveChanges();
                }

                librariesManager.Provider.SuppressSecurityChecks = false;
            }
        }

        private Guid CreateImageNativeAPI(string imageTitle, Stream imageStream, string imageExtension, LibrariesManager librariesManager)
        {

            Album album = librariesManager.GetAlbums().Where(al => al.Title == "Facebook Users").FirstOrDefault();
            if (album == null)
            {
                Guid albumId = Guid.NewGuid();
                
                album = librariesManager.GetAlbums().Where(a => a.Id == albumId).FirstOrDefault();

                if (album == null)
                {
                    //Create the album.
                    album = librariesManager.CreateAlbum(albumId);

                    //Set the properties of the album.
                    album.Title = "Facebook Users";
                    album.DateCreated = DateTime.UtcNow;
                    album.LastModified = DateTime.UtcNow;
                    album.UrlName = Regex.Replace("Facebook Users".ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");

                    //Save the changes.
                    librariesManager.SaveChanges();
                }

                album = librariesManager.GetAlbum(albumId);
            }
                
                //The album post is created as master. The masterImageId is assigned to the master version.
                Image image = librariesManager.CreateImage();
             

                librariesManager.SaveChanges();
                librariesManager.Provider.SuppressSecurityChecks = true;
                //Set the parent album.
               
                

                //Set the properties of the profile image
                image.Title = imageTitle;
                image.DateCreated = DateTime.UtcNow;
                image.PublicationDate = DateTime.UtcNow;
                image.LastModified = DateTime.UtcNow;
                image.UrlName = Regex.Replace(imageTitle.ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");

                librariesManager.Provider.SuppressSecurityChecks = true;
                image.Parent = album;

                //Upload the image file.
                librariesManager.Upload(image, imageStream, imageExtension);

                //Save the changes.
                librariesManager.SaveChanges();

                //Publish the Albums item. The live version acquires new ID.
                var bag = new Dictionary<string, string>();
                bag.Add("ContentType", typeof(Image).FullName);
                WorkflowManager.MessageWorkflow(image.Id, typeof(Image), null, "Publish", false, bag);

                return image.Id;
            
            
        }

        private void CreateAlbumNativeAPI(Guid albumId, string albumTitle)
        {
            
        }
    }
}