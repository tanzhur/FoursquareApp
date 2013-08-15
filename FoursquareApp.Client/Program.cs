using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoursquareApp.Models;
using FoursquareApp.Data;
using Spring.Social.Dropbox.Connect;
using Spring.Social.Dropbox.Api;

namespace FoursquareApp.Client
{
    class Program
    {
        private const string DropboxAppKey = "qo8v1rcsno3proe";
        private const string DropboxAppSecret = "y1djbdiyn46560r";

        private const string OAuthTokenFileName = "OAuthTokenFileName.txt";

        static void Main(string[] args)
        {
           // FoursquareContext testContext = new FoursquareContext();
            
            //testContext.Users.Add(new User() { Username = "MegaTest" });
            //testContext.SaveChanges();

            //foreach (var item in testContext.Users.ToList())
            //{
            //    foreach (var p in item.Places)
            //    {
            //        Console.WriteLine(p.Name);
            //    }
            //}

            //Console.WriteLine("WTF!");

            string image = "img1.jpg";

            DropboxProvider dropboxProvider = new DropboxProvider();

            DropboxServiceProvider dropboxServiceProvider =
            new DropboxServiceProvider(DropboxAppKey, DropboxAppSecret, AccessLevel.AppFolder);

            IDropbox dropbox = dropboxProvider.Authenticate(dropboxServiceProvider, dropboxProvider, OAuthTokenFileName);

            dropboxProvider.TakeUrl(dropbox, image);
        }
    }
}
