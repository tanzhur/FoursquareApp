using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoursquareApp.Models
{
    public class Place
    {
        public Place()
        {
            this.Comments = new HashSet<Comment>();
            this.Users = new HashSet<User>();
            this.Images = new HashSet<string>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public HashSet<string> Images { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
