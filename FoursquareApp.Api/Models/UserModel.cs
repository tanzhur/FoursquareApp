using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FoursquareApp.Models;
using System.Runtime.Serialization;

namespace FoursquareApp.Api.Models
{
    [DataContract]
    public class UserModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string SessionKey { get; set; }

        [DataMember]
        public List<String> Places { get; set; }

        [DataMember]
        public int CurrentPlaceId { get; set; }

        [DataMember]
        public decimal Longitude { get; set; }

        [DataMember]
        public decimal Latitude { get; set; }
        
        public UserModel(User user)
        {
            this.Id = user.Id;
            this.Username = user.Username;
            this.Password = user.Password;
            this.SessionKey = user.SessionKey;
            this.Places = new List<string>();
            this.CurrentPlaceId = user.currentPlaceId;
            this.Longitude = user.Longitude;
            this.Latitude = user.Latitude;

            foreach (Place currentPlace in user.Places)
            {
                this.Places.Add(currentPlace.Name);
            }
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

    [DataContract]
    public class UserRegisterModel
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string AuthCode { get; set; }

        [DataMember]
        public decimal Longitude { get; set; }

        [DataMember]
        public decimal Latitude { get; set; }
    }

    [DataContract]
    public class UserLoginModel
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string AuthCode { get; set; }
        
        [DataMember]
        public decimal Longitude { get; set; }

        [DataMember]
        public decimal Latitude { get; set; }
    }

    [DataContract]
    public class UserLoggedModel
    {
        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public string SessionKey { get; set; }

        [DataMember]
        public decimal Longitude { get; set; }

        [DataMember]
        public decimal Latitude { get; set; }
    }

    [DataContract]
    public class UserPlaceModel
    {
        public UserPlaceModel(UserModel user, PlaceModel place)
        {
            this.User = user;
            this.Place = place;
        }

        [DataMember]
        public UserModel User { get; set; }

        [DataMember]
        public PlaceModel Place { get; set; }
    }
}