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
    public class Program
    {
       

        static void Main()
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

            string image = "img3.jpg";
            string address = "http://t3.gstatic.com/images?q=tbn:ANd9GcRKZhVfP7IJlgWw70_yZjTXdplh5lQiXfTLctlicL2w3p3b0ESVVg";

            string url = DropboxProvider.AttachToPlace(image, address);
            Console.WriteLine(url);
        }
    }
}
