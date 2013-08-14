using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FoursquareApp.Models;
using System.Runtime.Serialization;

namespace FoursquareApp.Api.Models
{
    [DataContract]
    public class CommentModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Content { get; set; }

        [DataMember]
        public DateTime PostTime { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int PlaceId { get; set; }

        [DataMember]
        public UserModel User { get; set; }

        [DataMember]
        public PlaceModel Place { get; set; }

        public CommentModel(Comment comment)
        {
            this.Id = comment.Id;
            this.Content = comment.Content;
            this.PostTime = comment.PostTime;
            this.UserId = comment.UserId;
            this.PlaceId = comment.PlaceId;
            this.User = new UserModel(comment.User);
            this.Place = new PlaceModel(comment.Place);
        }     
    }
}