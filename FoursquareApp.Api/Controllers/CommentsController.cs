using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FoursquareApp.Models;
using FoursquareApp.Repos;
using FoursquareApp.Api.Models;

namespace FoursquareApp.Api.Controllers
{
    public class CommentsController : ApiController
    {
        IRepository<Comment> commentsRepo;
        IRepository<User> usersRepo;
        IRepository<Place> placesRepo;

        public CommentsController(IRepository<Comment> inputCommentsRepo, IRepository<User> inputUsersRepo,
                                    IRepository<Place> inputPlacesRepo)
        {
            this.commentsRepo = inputCommentsRepo;
            this.usersRepo = inputUsersRepo;
            this.placesRepo = inputPlacesRepo;
        }

        [HttpPost]
        [ActionName("create")]
        public HttpResponseMessage CreateComment(string sessionKey, [FromBody]CommentRegisterModel inputComment)
        {
            User currentUser = usersRepo.All().Where(u => u.SessionKey == sessionKey).FirstOrDefault();
            
            Place currentPlace = placesRepo.All().Where(p => p.Id == inputComment.PlaceId).FirstOrDefault();

            if (currentUser == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "User - Session Key mismatch.");
            }
            else if (currentPlace == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Place wasn't found.");
            }

            Comment newComment = new Comment()
            {
                Content = inputComment.Content,
                Place = currentPlace,
                PlaceId = currentPlace.Id,
                User = currentUser,
                UserId = currentUser.Id,
                PostTime = DateTime.Now,
            };

            try 
            {
                commentsRepo.Add(newComment);
            }
            catch(Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.InnerException.ToString());
            }
             
            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        [ActionName("get-all")]
        public HttpResponseMessage GetAllMessages()
        {
            var allComments = commentsRepo.All().ToList();

            ICollection<CommentFlatModel> resultComments = new List<CommentFlatModel>();

            foreach (Comment currentComment in allComments)
            {
                resultComments.Add(new CommentFlatModel(currentComment));
            }
            
            return this.Request.CreateResponse(HttpStatusCode.OK, resultComments);
        }
    }
}
