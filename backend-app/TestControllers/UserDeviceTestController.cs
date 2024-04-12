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
        private readonly IUserDeviceRepository _userDeviceRepository;

        public UserDeviceTestController(
            IUserDeviceRepository userDeviceRepository

        ) {
            _userDeviceRepository = userDeviceRepository;
        }

        public ICollection<UserDevice> GetUserDevices() {
            var userDevices = _userDeviceRepository.GetUserDevices();
            return userDevices;
        }

        public ICollection<UserDevice> GetUserDevices(int userID) {
            var userDevices = _userDeviceRepository.GetUserDevices(userID);
            return userDevices;
        }

        public UserDevice? GetUserDevice(int userID, int deviceID) {
            var userDevice = _userDeviceRepository.GetUserDevice(userID, deviceID);
            return userDevice;  
        }

        public UserDevice CreateUserDevice(UserDevice newUserDevice) {            
            var badUserDevice = new UserDevice() {
                UserID = -1,
                DeviceID = -1
            };

            if(!_userDeviceRepository.UserDeviceExists(newUserDevice.UserID, newUserDevice.DeviceID)) {
                var deviceCreated = _userDeviceRepository.CreateUserDevice(newUserDevice);

                if(deviceCreated) {
                    return newUserDevice;
                } else {
                    return badUserDevice;
                }
            } 

            return badUserDevice;
        }

        public bool DeleteUserDevice(UserDevice userDevice) {            
            if(!_userDeviceRepository.UserDeviceExists(userDevice.UserID, userDevice.DeviceID)) {
                var userDeleted = _userDeviceRepository.DeleteUserDevice(userDevice);
                return userDeleted;
            } 

            return false;
        }
        public List<User> GetUsersFromDevice() {
            var Users = new List<User>() {
                new User() {
                    UserID = 1,
                    UserType = "ADMIN", 
                    UserFirstName = "Han",
                    UserLastName = "Solo",
                    UserPassword = "123"
                },
                new User() {
                    UserID = 2,
                    UserType = "BASIC", 
                    UserFirstName = "Luke",
                    UserLastName = "Skywalker",
                    UserPassword = "456"
                }
            };
            return Users;
        }
    }
}