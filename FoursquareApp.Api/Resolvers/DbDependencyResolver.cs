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
        private static FoursquareContext dbContext = new FoursquareContext();
        private static IRepository<Place> placesRepo = new EfRepository<Place>(dbContext);
        private static IRepository<Comment> commentsRepo = new EfRepository<Comment>(dbContext);
        private static IRepository<User> usersRepo = new EfRepository<User>(dbContext);

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