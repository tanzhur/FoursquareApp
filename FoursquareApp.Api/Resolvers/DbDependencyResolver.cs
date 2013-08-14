using FoursquareApp.Data;
using FoursquareApp.Models;
using FoursquareApp.Repos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http.Dependencies;
using FoursquareApp.Api.Controllers;

namespace FoursquareApp.Api.Resolvers
{
    public class DbDependencyResolver : IDependencyResolver
    {
        // Note this shit is not tested
        private FoursquareContext dbContext;
        private IRepository<Place> placesRepo;
        private IRepository<Comment> commentsRepo;
        private IRepository<User> usersRepo;

        public DbDependencyResolver()
        {
            dbContext = new FoursquareContext();
            placesRepo = new EfRepository<Place>(dbContext);
            commentsRepo = new EfRepository<Comment>(dbContext);
            usersRepo = new EfRepository<User>(dbContext);
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(PlacesController))
            {
                return new PlacesController(placesRepo, usersRepo);
            }
            else if (serviceType == typeof(UsersController))
            {
                return new UsersController(usersRepo, placesRepo);
            }
            else if(serviceType == typeof(CommentsController))
            {
                return new CommentsController(commentsRepo, usersRepo, placesRepo);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose()
        {
        }
    }
}