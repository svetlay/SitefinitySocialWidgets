This is a beta Facebook login widget

The pacakge manager will install this package in the MVC folder of the sitefinity instanllation

Make sure to 
- copy the contents of global_merge.asax to your global asax
- Run the project in IIS
- Go to Advanced Settings -> facebook and set up your app details. Make sure your facebook app has the correct URL
- Create a library called facebook users and allow annonymous users to manage the contents of that library(for avatar upload)
- add text fields called "Birthday" and "Location" in the user profile to make use of this data from Facebook
- have in mind that the passwords are generated automatically, before placing this in production you can decide whether to use encryption of data for password generation or allow the users to have their own passwords within your app
- Enjoy

Created by Svetla Yankova







