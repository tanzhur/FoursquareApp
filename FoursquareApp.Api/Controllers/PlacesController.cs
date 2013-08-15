using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FoursquareApp.Models;
using FoursquareApp.Repos;
using FoursquareApp.Api.Models;
using FoursquareApp.Client;

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
            Place existingPlace = placesRepo.All().Where(p => p.Longitude == place.Longitude && p.Latitude == place.Latitude).FirstOrDefault();
            
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

        [HttpGet]
        [ActionName("get-all")]
        public HttpResponseMessage GetAllPlaces()
        {
            ICollection<PlaceModel> resultPlaces = new List<PlaceModel>();

            var allPlaces = placesRepo.All();

            foreach (Place currentPlace in allPlaces)
            {
                PlaceModel currentModelPlace = new PlaceModel(currentPlace);

                resultPlaces.Add(currentModelPlace);
            }

            if (resultPlaces.Count == 0)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NoContent, "There aren't any places yet");
            }
            else
            {
                return this.Request.CreateResponse(HttpStatusCode.OK, resultPlaces);
            }
        }

        [HttpGet]
        [ActionName("get-current")]
        public HttpResponseMessage GetCurrentUserPlaces(string sessionKey)
        {
            User currentUser = usersRepo.All().Where(u => u.SessionKey == sessionKey).FirstOrDefault();

            if (currentUser == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "There isn't such logged user.");
            }

            ICollection<PlaceModel> resultPlaces = new List<PlaceModel>();

            foreach (Place currentPlace in currentUser.Places)
            {
                PlaceModel currentPlaceModel = new PlaceModel(currentPlace);

                resultPlaces.Add(currentPlaceModel);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, resultPlaces);
        }

        [HttpGet]
        [ActionName("get-closest")]
        public HttpResponseMessage GetCurrentUserClosestPlaces(string sessionKey)
        {
            ICollection<PlaceModel> allPlaceModels = new List<PlaceModel>();

            var allPlaces = placesRepo.All().ToList();

            foreach (Place currentPlace in allPlaces)
            {
                PlaceModel currentModelPlace = new PlaceModel(currentPlace);

                allPlaceModels.Add(currentModelPlace);
            }
            
            User currentUser = usersRepo.All().Where(u => u.SessionKey == sessionKey).FirstOrDefault();

            if (currentUser == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "There isn't such logged user.");
            }

            ICollection<PlaceModel> resultPlaceModels = new List<PlaceModel>();

            foreach (Place currentPlace in allPlaces)
            {
                if (Math.Abs(currentUser.Longitude - currentPlace.Longitude) <= 1 && 
                        Math.Abs(currentUser.Latitude - currentPlace.Latitude) <= 1)
                {
                    PlaceModel currentPlaceModel = new PlaceModel(currentPlace);

                    resultPlaceModels.Add(currentPlaceModel);
                }
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, resultPlaceModels);
        }
    }
}
