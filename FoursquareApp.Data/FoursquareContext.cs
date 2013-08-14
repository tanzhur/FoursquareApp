using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoursquareApp.Models;

namespace FoursquareApp.Data
{
    public class FoursquareContext : DbContext
    {
        public FoursquareContext()
            : base("FoursquareDb")
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Place> Places { get; set; }
    }
}
