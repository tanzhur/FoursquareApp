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
    public class PlacesController : ApiController
    {
        IRepository<Place> placesRepo;
        IRepository<User> usersRepo;

        public PlacesController(IRepository<Place> placesRepo, IRepository<User> usersRepo)
        {
            this.placesRepo = placesRepo;
            this.usersRepo = usersRepo;
        }

        [HttpPost]
        [ActionName("create")]
        public HttpResponseMessage CreatePlace(string sessionKey, [FromBody] PlaceModelRegister place)
        {
            // TODO: 
            Place existingPlace = placesRepo.All().Where(p => p.Longitude == place.Longitude && p.Longitude == place.Longitude).FirstOrDefault();
            if (existingPlace != null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "This place already exists.");
            }

            User user = usersRepo.All().Where(u => u.SessionKey == sessionKey).FirstOrDefault();
            if (user == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Cannot create place. Session key and user don't match.");
            }

            Place pl = new Place()
            {
                Name = place.Name,
                Latitude = place.Latitude,
                Longitude = place.Longitude
            };

            pl.Users.Add(user);
            placesRepo.Add(pl);
            return this.Request.CreateResponse(HttpStatusCode.OK, place);
        }

        //[HttpGet]
        //[ActionName("get-all")]
        //public HttpResponseMessage GetPlaces(string sessionKey)
        //{
            
        //}
    }
}
