using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FoursquareApp.Models;

namespace FoursquareApp.Api.Models
{
    public class PlaceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public HashSet<string> Images { get; set; }

        public PlaceModel(Place place)
        {
            this.Id = place.Id;
            this.Name = place.Name;
            this.Latitude = place.Latitude;
            this.Longitude = place.Longitude;
        }
    }

    public class PlaceDetails : PlaceModel
    {
        public ICollection<CommentModel> Comments = new List<CommentModel>();

        public ICollection<UserModel> Users = new List<UserModel>();

        public PlaceDetails(Place place)
            : base(place)
        {
            foreach (var comment in place.Comments)
            {
                Comments.Add(new CommentModel(comment));
            }

            foreach (var user in place.Users)
            {
                Users.Add(new UserModel(user));
            }
        }
    }

    public class PlaceModelRegister
    {
        public string Name { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }
    }
}