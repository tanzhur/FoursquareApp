using System;
using System.IO;
using System.Diagnostics;

using Spring.Social.OAuth1;
using Spring.Social.Dropbox.Api;
using Spring.Social.Dropbox.Connect;
using Spring.IO;

namespace FoursquareApp.Client
{
    public class DropboxProvider
    {
        public DropboxProvider()
        { }

        public OAuthToken LoadOAuthToken(string OAuthTokenFileName)
        {
            string[] lines = File.ReadAllLines(OAuthTokenFileName);
            OAuthToken oauthAccessToken = new OAuthToken(lines[0], lines[1]);
            return oauthAccessToken;
        }

        public void AuthorizeAppOAuth(DropboxServiceProvider dropboxServiceProvider, string OAuthTokenFileName)
        {
            // Authorization without callback url
            Console.Write("Getting request token...");
            OAuthToken oauthToken = dropboxServiceProvider.OAuthOperations.FetchRequestToken(null, null);
            Console.WriteLine("Done.");

            OAuth1Parameters parameters = new OAuth1Parameters();
            string authenticateUrl = dropboxServiceProvider.OAuthOperations.BuildAuthorizeUrl(
                oauthToken.Value, parameters);
            Console.WriteLine("Redirect the user for authorization to {0}", authenticateUrl);
            Process.Start(authenticateUrl);
            Console.Write("Press [Enter] when authorization attempt has succeeded.");
            Console.ReadLine();

            Console.Write("Getting access token...");
            AuthorizedRequestToken requestToken = new AuthorizedRequestToken(oauthToken, null);
            OAuthToken oauthAccessToken =
                dropboxServiceProvider.OAuthOperations.ExchangeForAccessToken(requestToken, null);
            Console.WriteLine("Done.");

            string[] oauthData = new string[]
            {
                oauthAccessToken.Value,
                oauthAccessToken.Secret
            };
            File.WriteAllLines(OAuthTokenFileName, oauthData);
        }

        public IDropbox Authenticate(DropboxServiceProvider dropboxServiceProvider, DropboxProvider provider, string OAuthTokenFileName)
        {
            // Authenticate the application (if not authenticated) and load the OAuth token
            if (!File.Exists(OAuthTokenFileName))
            {
                provider.AuthorizeAppOAuth(dropboxServiceProvider, OAuthTokenFileName);
            }
            OAuthToken oauthAccessToken = provider.LoadOAuthToken(OAuthTokenFileName);

            // Login in Dropbox
            IDropbox dropbox = dropboxServiceProvider.GetApi(oauthAccessToken.Value, oauthAccessToken.Secret);
            return dropbox;
        }

        public void TakeUrl(IDropbox dropbox, string image)
        {

            // Create new folder
            // string newFolderName = "Test";
            // Entry createFolderEntry = dropbox.CreateFolder(newFolderName);
            //Console.WriteLine("Created folder: {0}", createFolderEntry.Path);

            // Upload a file
            Entry uploadFileEntry = dropbox.UploadFile(new FileResource("../../TestImages/"+image), "/" + image);

            // take url
            DropboxLink sharedUrl = dropbox.GetShareableLink(uploadFileEntry.Path);
            Console.WriteLine(sharedUrl.Url);
        }
    }
}