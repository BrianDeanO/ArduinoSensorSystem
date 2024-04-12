using AutoMapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using backEndApp.DTO;
using backEndApp.Interfaces;
using backEndApp.Models;
using System.Text.Json;

namespace backEndApp.TestControllers {
    public class UserTestController : Controller {
        private readonly IUserRepository _userRepository;

        public UserTestController(
            IUserRepository userRepository

        ) {
            _userRepository = userRepository;
        }

        public ICollection<User> GetUsers() {
            var users = _userRepository.GetUsers();
            return users;
        }

        public User? GetUser(int userID) {
            var user = _userRepository.GetUser(userID);
            return user;  
        }

        public ICollection<User> GetAdminUsers() {
            var users = _userRepository.GetAdminUsers();
            return users;  
        }

        public User? GetUserWithLogin(
            string userFirstName, 
            string userLastName,
            string userPassword
        ) {
            var user = _userRepository.GetUserWithLogin(
                userFirstName, 
                userLastName,
                userPassword
            );
            return user;  
        }

        public User CreateUser(User newUser) {            
            var badUser = new User() {
                UserID = -1,
                UserType = "BAD", 
                UserFirstName = "BAD",
                UserLastName = "BAD",
                UserPassword = "BAD"
            };

            if(!_userRepository.UserExists(newUser.UserID)) {

                newUser.UserIsDeleted = false;

                var deviceCreated = _userRepository.CreateUser(newUser);
                if(deviceCreated) {
                    return newUser;
                } else {
                    return badUser;
                }
            } 

            return badUser;
        }

        public User UpdateUser(User newUser, string newUserType) {            
            var badUser = new User() {
                UserID = -1,
                UserType = "BAD", 
                UserFirstName = "BAD",
                UserLastName = "BAD",
                UserPassword = "BAD"
            };

            if(!_userRepository.UserExists(newUser.UserID)) {
                newUser.UserType = newUserType;

                var userUpdated = _userRepository.UpdateUser(newUser);
                if(userUpdated) {
                    return newUser;
                } else {
                    return badUser;
                }
            } 

            return badUser;
        }

        public bool DeleteUser(User user) {            
            if(!_userRepository.UserExists(user.UserID)) {
                var userDeleted = _userRepository.DeleteUser(user);
                return userDeleted;
            } 

            return false;
        }
    }
}