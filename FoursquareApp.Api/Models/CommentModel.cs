using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FoursquareApp.Models;

namespace FoursquareApp.Api.Models
{
    public class CommentModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime PostTime { get; set; }

        public int UserId { get; set; }

        public int PlaceId { get; set; }
       
        public User User { get; set; }

        public Place Place { get; set; }

        public CommentModel(Comment comment)
        {
            this.Id = comment.Id;
            this.Content = comment.Content;
            this.PostTime = comment.PostTime;
            this.UserId = comment.UserId;
            this.PlaceId = comment.PlaceId;
            this.User = comment.User;
            this.Place = comment.Place;
        }     
    }
}