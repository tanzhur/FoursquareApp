using System;
using System.IO;
using System.Diagnostics;

using Spring.Social.OAuth1;
using Spring.Social.Dropbox.Api;
using Spring.Social.Dropbox.Connect;
using Spring.IO;
using System.Net.Http;

namespace FoursquareApp.Client
{
    public class DropboxProvider
    {
        public DropboxProvider()
        { }

        private const string DropboxAppKey = "qo8v1rcsno3proe";
        private const string DropboxAppSecret = "y1djbdiyn46560r";

        private const string OAuthTokenFileName = "OAuthTokenFileName.txt";

        public OAuthToken LoadOAuthToken(string OAuthTokenFileName)
        {
            string[] lines = File.ReadAllLines(OAuthTokenFileName);
            OAuthToken oauthAccessToken = new OAuthToken(lines[0], lines[1]);
            return oauthAccessToken;
        }

        public void AuthorizeAppOAuth(DropboxServiceProvider dropboxServiceProvider, string OAuthTokenFileName)
        {
          
            OAuthToken oauthToken = dropboxServiceProvider.OAuthOperations.FetchRequestToken(null, null);

           OAuth1Parameters parameters = new OAuth1Parameters();
            string authenticateUrl = dropboxServiceProvider.OAuthOperations.BuildAuthorizeUrl(
                oauthToken.Value, parameters);
       
            //Process.Start(authenticateUrl);
         
         
            AuthorizedRequestToken requestToken = new AuthorizedRequestToken(oauthToken, null);
            OAuthToken oauthAccessToken =
                dropboxServiceProvider.OAuthOperations.ExchangeForAccessToken(requestToken, null);

            string[] oauthData = new string[]
            {
                oauthAccessToken.Value,
                oauthAccessToken.Secret
            };
            File.WriteAllLines(OAuthTokenFileName, oauthData);
        }

        private IDropbox Authenticate(DropboxServiceProvider dropboxServiceProvider, DropboxProvider provider, string OAuthTokenFileName)
        {
            OAuthToken oauthAccessToken = new OAuthToken("ng8ydj1aoljno9fk", "fyqdn9sv71fn8on");

            // Login in Dropbox
            IDropbox dropbox = dropboxServiceProvider.GetApi(oauthAccessToken.Value, oauthAccessToken.Secret);

            return dropbox;
        }

        public static string AttachToPlace(string image, string address)
        {
            DropboxProvider dropboxProvider = new DropboxProvider();

            DropboxServiceProvider dropboxServiceProvider =
            new DropboxServiceProvider(DropboxAppKey, DropboxAppSecret, AccessLevel.AppFolder);

            IDropbox dropbox = dropboxProvider.Authenticate(dropboxServiceProvider, dropboxProvider, OAuthTokenFileName);

            string resultUrl = dropboxProvider.TakeUrl(dropbox, image, address);
            return resultUrl;
        }

        private string TakeUrl(IDropbox dropbox, string image, string address)
        {
            
            // Create new folder
            // string newFolderName = "Test";
            // Entry createFolderEntry = dropbox.CreateFolder(newFolderName);
            //Console.WriteLine("Created folder: {0}", createFolderEntry.Path);

            HttpClient client = new HttpClient();

            byte[] imageAsByteArr = client.GetByteArrayAsync(address).Result;

            Entry uplodeImage = dropbox.UploadFile(new ByteArrayResource(imageAsByteArr), "/" + image);
            // Upload a file
            // Entry uploadFileEntry = dropbox.UploadFile(new FileResource("../../TestImages/" + image), "/" + image);

            // take url
            DropboxLink link = dropbox.GetMediaLink(uplodeImage.Path);
            string url = link.Url;
            return url;
        }
    }
}