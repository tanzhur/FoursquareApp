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
        private IRepository<Image> imageRepo;

        public DbDependencyResolver()
        {
            this.dbContext = new FoursquareContext();
            this.placesRepo = new EfRepository<Place>(dbContext);
            this.commentsRepo = new EfRepository<Comment>(dbContext);
            this.usersRepo = new EfRepository<User>(dbContext);
            this.imageRepo = new EfRepository<Image>(dbContext);
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
            else if(serviceType == typeof(ImagesController))
            {
                return new ImagesController(imageRepo, usersRepo, placesRepo);
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