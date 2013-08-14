﻿using FoursquareApp.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FoursquareApp.Models;
using System.Text;
using System.Security.Cryptography;
using FoursquareApp.Api.Models;
using FoursquareApp.Api.InternalControllers;

namespace FoursquareApp.Api.Controllers
{
    public class UsersController : ApiController
    {

        private const string SessionKeyChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const int SessionKeyLen = 50;
        protected static Random rand = new Random();

        private IRepository<User> userRepo;

        /// Constructor
        public UsersController(IRepository<User> inputRepo)
        {
            this.userRepo = inputRepo;
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

        /// Public methods for requests
        [HttpPost]
        [ActionName("register")]
        public HttpResponseMessage RegisterUser([FromBody]UserRegisterModel inputUser)
        {
            if (inputUser.Username == null || inputUser.Username == "")//|| inputUser.AuthCode.Length != 40)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Username or password is invalid");
            }

            User foundedUser = userRepo.All().Where(u => u.Username == inputUser.Username);

            if (foundedUser != null)
            {
                return this.Request.CreateErrorResponse(HttpStatusCode.Conflict, "There is already a user with this username");
            }

            User userToBeRegistered = new User()
            {
                Username = inputUser.Username,
                Password = inputUser.AuthCode
            };

            userRepo.Add(userToBeRegistered);

            UserLoggedModel loggedUser = new UserLoggedModel()
            {
                UserName = userToBeRegistered.Username,
                SessionKey = GenerateSessionKey(userToBeRegistered.Id)
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

            UserLoggedModel loggedUser = new UserLoggedModel()
            {
                UserName = currentUser.Username,
                SessionKey = GenerateSessionKey(currentUser.Id)
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

            currentUser.SessionKey = null;

            userRepo.Update(currentUser.Id, currentUser);

            return this.Request.CreateResponse(HttpStatusCode.OK);
        }

        //[HttpPost]
        //[ActionName("comment")]
        //public HttpResponseMessage CommentPlace([FromBody]Comment inputComment, int userId, int placeId)
        //{
        //}

        //[HttpPost]
        //[ActionName("check-in")]
        //public HttpResponseMessage CheckInPlace(int userId, int placeId)
        //{
        //    UserController.CheckInPlace(userId, placeId);
        //}
    }
}
