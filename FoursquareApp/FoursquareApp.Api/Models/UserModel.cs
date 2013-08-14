using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FoursquareApp.Models;

namespace FoursquareApp.Api.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public UserModel(User user)
        {
            this.Id = user.Id;
            this.Username = user.Username;
            this.Password = user.Password;
        }
    }

    public class UserDetails : UserModel
    {
        public ICollection<PlaceModel> Places = new List<PlaceModel>();

        public ICollection<CommentModel> Comments = new List<CommentModel>();

        public UserDetails(User user)
            : base(user)
        {
            foreach (var place in user.Places)
            {
                Places.Add(new PlaceModel(place));
            }

            foreach (var comment in user.Comments)
            {
                Comments.Add(new CommentModel(comment));
            }

        }
    }
}