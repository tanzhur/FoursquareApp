using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FoursquareApp.Models;
using FoursquareApp.Repos;

namespace FoursquareApp.Api.Controllers
{
    public class CommentsController : ApiController
    {
        IRepository<Comment> commentsRepo;

        public CommentsController(IRepository<Comment> inputRepo)
        {
            this.commentsRepo = inputRepo;
        }
    }
}
