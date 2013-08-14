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
    public class PlacesController : ApiController
    {
        IRepository<Place> placesRepo;

        public PlacesController(IRepository<Place> inputRepo)
        {
            this.placesRepo = inputRepo;
        }
    }
}
