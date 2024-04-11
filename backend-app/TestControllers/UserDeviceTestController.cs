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
    public class UserDeviceTestController : Controller {
        private readonly IUserRepository _userRepository;
        // private readonly ISensorRepository _sensorRepository;
        private readonly List<Device> _deviceDbContext;
        private readonly List<User> _userDbContext;
        private readonly List<Sensor> _sensorDbContext;
        private readonly List<SensorData> _sensorDataDbContext;
        private readonly List<SensorConfig> _sensorConfigDbContext;
        private readonly List<UserDevice> _userDeviceDbContext;

        public UserDeviceTestController(
            IUserRepository userRepository,
            // ISensorRepository sensorRepository,
            List<Device> deviceDbContext,
            List<User> userDbContext,
            List<Sensor> sensorDbContext,
            List<SensorData> sensorDataDbContext,
            List<SensorConfig> sensorConfigDbContext,
            List<UserDevice> userDeviceDbContext

        ) {
            _userRepository = userRepository;
            // _sensorRepository = sensorRepository;
            _deviceDbContext = deviceDbContext;
            _userDbContext = userDbContext;
            _sensorDbContext = sensorDbContext;
            _sensorDataDbContext = sensorDataDbContext;
            _sensorConfigDbContext = sensorConfigDbContext;
            _userDeviceDbContext = userDeviceDbContext;
        }

        public ICollection<User> GetUsers() {
            var users = _userRepository.GetUsers();
            return users;
        }

        public User? GetUser(int userID) {
            var user = _userRepository.GetUser(userID);
            return user;  
        }

        // public List<UserDevice> GetUserDevices() {
        //     List<UserDevice> userDevices = new List<UserDevice>() {
        //         new UserDevice() {
        //             UserID = 1,
        //             DeviceID = 1
        //         },
        //         new UserDevice() {
        //             UserID = 1,
        //             DeviceID = 2
        //         }
        //     };
        //     return userDevices;
        // }

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
        // public List<User> GetUsersFromDevice() {
        //     var Users = new List<User>() {
        //         new User() {
        //             UserID = 1,
        //             UserType = "ADMIN", 
        //             UserFirstName = "Han",
        //             UserLastName = "Solo",
        //             UserPassword = "123"
        //         },
        //         new User() {
        //             UserID = 2,
        //             UserType = "BASIC", 
        //             UserFirstName = "Luke",
        //             UserLastName = "Skywalker",
        //             UserPassword = "456"
        //         }
        //     };
        //     return Users;
        // }
    }
}