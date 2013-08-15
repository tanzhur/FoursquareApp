using FoursquareApp.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FoursquareApp.Models;
using System.Text;
using FoursquareApp.Api.Models;
using System.Configuration;
using Newtonsoft.Json;

namespace FoursquareApp.Api.Controllers
{
    public class UsersController : ApiController
    {
        private readonly PubnubAPI pubnub = new PubnubAPI(
            "pub-c-dc19a1e8-4dd2-4c2d-8099-1f73b901d609",               // PUBLISH_KEY
            "sub-c-abf55e18-0580-11e3-991c-02ee2ddab7fe",               // SUBSCRIBE_KEY
            "sec-c-ZTJkZDRlYTUtNTBlNC00N2M4LTgzYTctOTliNmFhYTkxNGU2",   // SECRET_KEY
            true                                                        // SSL_ON?
        );
        private readonly string channel = "userplaces-channel";

     
        
        private const string SessionKeyChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int SessionKeyLen = 50;
        protected static Random rand = new Random();

        private IRepository<User> userRepo;
        private IRepository<Place> placeRepo;
        
        public UsersController(IRepository<User> inputUserRepo, IRepository<Place> inputPlaceRepo)
        {
            this.userRepo = inputUserRepo;
            this.placeRepo = inputPlaceRepo;
        }

        private static string GenerateSessionKey(int userId)
        {
            StringBuilder keyChars = new StringBuilder(50);
            keyChars.Append(userId.ToString());
            while (keyChars.Length < SessionKeyLen)
            {
                int randomCharNum;
                lock (rand)
                {
                    randomCharNum = rand.Next(SessionKeyChars.Length);
                }
                char randomKeyChar = SessionKeyChars[randomCharNum];
                keyChars.Append(randomKeyChar);
            }
            string sessionKey = keyChars.ToString();
            return sessionKey;
        }

        private void SendPubnubMessage(UserPlaceModel item)
        {
            this.pubnub.Publish(this.channel, item);
        }
        
        [HttpPost]
        [ActionName("register")]
        public HttpResponseMessage RegisterUser([FromBody]UserRegisterModel inputUser)
        {
            if (string.IsNullOrWhiteSpace(inputUser.Username) || 
                string.IsNullOrWhiteSpace(inputUser.AuthCode) || 
                inputUser.AuthCode.Length != 40)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Username or password is invalid");
            }

            User foundedUser = userRepo.All().Where(u => u.Username == inputUser.Username).FirstOrDefault();

            if (foundedUser != null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.Conflict, "There is already a user with this username");
            }

            User userToBeRegistered = new User()
            {
                Username = inputUser.Username,
                Password = inputUser.AuthCode,
                Latitude = inputUser.Latitude,
                Longitude = inputUser.Longitude
            };

            userRepo.Add(userToBeRegistered);

            UserLoggedModel loggedUser = new UserLoggedModel()
            {
                UserName = userToBeRegistered.Username,
                SessionKey = GenerateSessionKey(userToBeRegistered.Id),
                Latitude = inputUser.Latitude,
                Longitude = inputUser.Longitude
            };

            userToBeRegistered.SessionKey = loggedUser.SessionKey;
            userRepo.Update(userToBeRegistered.Id, userToBeRegistered);
            return this.Request.CreateResponse(HttpStatusCode.OK, loggedUser);
        }

        [HttpPost]
        [ActionName("login")]
        public HttpResponseMessage LoginUser([FromBody]UserLoginModel inputUser)
        {
            User currentUser = userRepo.All().Where(u => u.Username == inputUser.Username).FirstOrDefault();

            if (currentUser == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound,
                    string.Format("User with username : {0}, doesn't exist ", inputUser.Username));
            }
            else if (inputUser.AuthCode != currentUser.Password)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Username or password don't match");
            }
            else if (currentUser.SessionKey != null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User already logged in.");
            }

            UserLoggedModel loggedUser = new UserLoggedModel()
            {
                UserName = currentUser.Username,
                SessionKey = GenerateSessionKey(currentUser.Id),
                Latitude = currentUser.Latitude,
                Longitude = currentUser.Longitude
            };

            currentUser.SessionKey = loggedUser.SessionKey;
            
            userRepo.Update(currentUser.Id, currentUser);

            return this.Request.CreateResponse(HttpStatusCode.OK, loggedUser);
        }

        [HttpGet]
        [ActionName("logout")]
        public HttpResponseMessage LogoutUser(string sessionKey)
        {
            User currentUser = userRepo.All().Where(u => u.SessionKey == sessionKey).FirstOrDefault();

            if (currentUser == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User is not logged in or does not exist!");
            }

            currentUser.SessionKey = null;

            userRepo.Update(currentUser.Id, currentUser);

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        [ActionName("get-all")]
        public HttpResponseMessage AllUsers()
        {
            if (ConfigurationManager.AppSettings["get-all-users"] == "true")
            {
                var allUsers = userRepo.All().ToList();

                ICollection<UserModel> resultUsers = new List<UserModel>();

                foreach (User user in allUsers)
                {
                    resultUsers.Add(new UserModel(user));
                }

                return this.Request.CreateResponse(HttpStatusCode.OK, resultUsers);
            }
            else
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "You can't view all users");
            }
            
        }

        [HttpPost]
        [ActionName("check-in")]
        public HttpResponseMessage CheckInPlace(string sessionKey,[FromBody]int? PlaceId)
        {
            User currentUser = userRepo.All().Where(u => u.SessionKey == sessionKey).FirstOrDefault();

            Place currentPlace = placeRepo.All().Where(p => p.Id == PlaceId).FirstOrDefault();
            
            if (currentUser == null || currentPlace == null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.NotFound, "User or Place wasn't found.");
            }

            currentUser.currentPlaceId = currentPlace.Id;
            currentUser.Latitude = currentPlace.Latitude;
            currentUser.Longitude = currentPlace.Longitude;

            userRepo.Update(currentUser.Id, currentUser);

            //PubNub information sending 

            UserPlaceModel item = new UserPlaceModel(new UserModel(currentUser), new PlaceModel(currentPlace));
            
            SendPubnubMessage(item);

            return this.Request.CreateResponse(HttpStatusCode.OK, string.Format("You successfully checked in : {0} !", currentPlace.Name));
        }

    }
}
