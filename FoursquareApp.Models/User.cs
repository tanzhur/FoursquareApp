using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoursquareApp.Models
{
    public class User
    {
        public User()
        {
            this.Places = new HashSet<Place>();
            this.Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string SessionKey { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public int  currentPlaceId { get; set; }

        public virtual ICollection<Place> Places { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
