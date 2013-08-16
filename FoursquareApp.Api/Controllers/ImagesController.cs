using FoursquareApp.Api.Models;
using FoursquareApp.Client;
using FoursquareApp.Models;
using FoursquareApp.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FoursquareApp.Api.Controllers
{
    public class ImagesController : ApiController
    {
        private IRepository<Image> imagesRepo;
        private IRepository<User> usersRepo;
        private IRepository<Place> placesRepo;

        public ImagesController(IRepository<Image> imageRepository,
            IRepository<User> usersRepo, IRepository<Place> placesRepo)
        {
            this.imagesRepo = imageRepository;
            this.usersRepo = usersRepo;
            this.placesRepo = placesRepo;
        }

        [HttpPost]
        [ActionName("attach-picture")]
        public HttpResponseMessage AttachImageToPlace(string sessionKey, [FromBody] PlaceImageAttach imageInformation)
        {
            User currentUser = usersRepo.All().Where(u => u.SessionKey == sessionKey).FirstOrDefault();
            if (currentUser == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Session key and user don't match");
            }

            Place currentPlace = placesRepo.All().Where(p => p.Id == imageInformation.PlaceId).FirstOrDefault();
            if (currentPlace == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The place does not exist!");
            }
            string url = "";
            try
            {

                url = DropboxProvider.AttachToPlace(imageInformation.ImageName, imageInformation.ImageUrl);
            }
            catch (Exception ex)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.InnerException.ToString());
            }
            url = DropboxProvider.AttachToPlace(imageInformation.ImageName, imageInformation.ImageUrl);
            imagesRepo.Add(new Image() { Url = url, PlaceId = currentPlace.Id });
            return this.Request.CreateResponse(HttpStatusCode.Created, url);
        }

        [HttpPost]
        [ActionName("get-all-pictures")]
        public HttpResponseMessage GetAllPictures([FromBody]int? PlaceId)
        {
            ICollection<string> allPicturesLinks = new List<string>();
            Place currentPlace = placesRepo.All().Where(p => p.Id == PlaceId).FirstOrDefault();

            var allImages = imagesRepo.All().Where(i => i.PlaceId == PlaceId).ToList();
            var imgs = imagesRepo.All().ToList();
            if (currentPlace == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Place not found");
            }

            foreach (Image currentImage in allImages)
            {
                allPicturesLinks.Add(currentImage.Url);
            }

            return this.Request.CreateResponse(HttpStatusCode.OK, allPicturesLinks);
        }
    }
}
