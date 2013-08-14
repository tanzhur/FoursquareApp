﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoursquareApp.Models;
using FoursquareApp.Data;

namespace FoursquareApp.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            FoursquareContext testContext = new FoursquareContext();

            //testContext.Users.Add(new User() { Username = "MegaTest" });
            //testContext.SaveChanges();

            foreach (var item in testContext.Users.ToList())
            {
                foreach (var p in item.Places)
                {
                    Console.WriteLine(p.Name);
                }
            }

            Console.WriteLine("WTF!");
        }
    }
}
