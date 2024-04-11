using System;
using Moq;
using Xunit;
using backEndApp.DTO;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;
using backEndApp.Controllers;
using backEndApp.TestControllers;
using backEndApp.UnitTests;
using backEndApp.Repository;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    public class TestController_User
    {
        private readonly Mock<IUserRepository> _IUserRepository;
        public List<Device> deviceList = UnitTestHelper.GetDevices(); 
        public List<User> userList = UnitTestHelper.GetUsers();
        public List<Sensor> sensorList = UnitTestHelper.GetSensors();
        public List<SensorData> sensorDataList = UnitTestHelper.GetSensorData();
        public List<SensorConfig> sensorConfigList = UnitTestHelper.GetSensorConfigs();
        public List<UserDevice> userDeviceList = UnitTestHelper.GetUserDevices();
        public TestController_User() {
            _IUserRepository = new Mock<IUserRepository>();
        }

        [Fact]
        public void GetAllUsers() {
            // Arrange
            _IUserRepository.Setup(x => x.GetUsers())
                .Returns(userList);
            
            var userController = new UserTestController(
                _IUserRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );
            // Act
            var usersResult = userController.GetUsers().ToArray();

            // Assert
            Assert.NotNull(usersResult);
            Assert.Equal(userList[0].UserID, usersResult[0].UserID);
            Assert.Equal(userList[1].UserID, usersResult[1].UserID);
            Assert.Equal(userList[2].UserID, usersResult[2].UserID);
        }
        [Fact]
        public void GetUserByID() {
            // Arrange
            var user = new User() {
                UserID = 1,
                UserType = "ADMIN", 
                UserFirstName = "Han",
                UserLastName = "Solo",
                UserPassword = "123"
            };

            _IUserRepository.Setup(x => x.GetUser(user.UserID))
                .Returns(user);
            
            var userController = new UserTestController(
                _IUserRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var userResult = userController.GetUser(user.UserID);

            // Assert
            Assert.NotNull(userResult);
            Assert.Equal(user.UserID, userResult.UserID);
        }

        [Fact]
        public void GetAdminUsers() {
            // Arrange
            var users = new List<User>() {
                new User {
                    UserID = 1,
                    UserType = "ADMIN", 
                    UserFirstName = "Han",
                    UserLastName = "Solo",
                    UserPassword = "123"
                }
            };

            _IUserRepository.Setup(x => x.GetAdminUsers())
                .Returns(users);
            
            var userController = new UserTestController(
                _IUserRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var userResult = userController.GetAdminUsers().ToArray();

            // Assert
            Assert.NotNull(userResult);
            Assert.Equal(users.Count, userResult.Length);
            Assert.Equal(users[0].UserID, userResult[0].UserID);
            Assert.Equal("ADMIN", userResult[0].UserType);
        }

        [Fact]
        public void GetUserWithLogin() {
            // Arrange
            var user = new User() {
                UserID = 1,
                UserType = "ADMIN", 
                UserFirstName = "Han",
                UserLastName = "Solo",
                UserPassword = "123"
            };

            _IUserRepository.Setup(x => x.GetUserWithLogin(
                    user.UserFirstName,
                    user.UserLastName,
                    user.UserPassword
                ))
                .Returns(user);
            
            var userController = new UserTestController(
                _IUserRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var userResult = userController.GetUserWithLogin(
                user.UserFirstName,
                user.UserLastName,
                user.UserPassword
            );

            // Assert
            Assert.NotNull(userResult);
            Assert.Equal(user.UserID, userResult.UserID);
        }

        [Fact]
        public void CreateUser() {
            // Arrange
            var user = new User() {
                UserID = 1,
                UserType = "ADMIN", 
                UserFirstName = "Han",
                UserLastName = "Solo",
                UserPassword = "123"
            };
            _IUserRepository.Setup(x => x.CreateUser(user))
                .Returns(true);
            
            var userController = new UserTestController(
                _IUserRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var userResult = userController.CreateUser(user);

            // Assert
            Assert.NotNull(userResult);
            Assert.Equal(user.UserID, userResult.UserID);
            Assert.False(userResult.UserIsDeleted);
        }

        [Fact]
        public void UpdateUser() {
            // Arrange
            var user = new User() {
                UserID = 1,
                UserType = "ADMIN", 
                UserFirstName = "Han",
                UserLastName = "Solo",
                UserPassword = "123"
            };
            var newUserType = "BASIC";

            _IUserRepository.Setup(x => x.UpdateUser(user))
                .Returns(true);
            
            var userController = new UserTestController(
                _IUserRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var userResult = userController.UpdateUser(user, newUserType);

            // Assert
            Assert.NotNull(userResult);
            Assert.Equal(user.UserID, userResult.UserID);
            Assert.Equal(newUserType, userResult.UserType);
        }

        [Fact]
        public void DeleteUser() {
            // Arrange
            var user = new User() {
                UserID = 1,
                UserType = "ADMIN", 
                UserFirstName = "Han",
                UserLastName = "Solo",
                UserPassword = "123"
            };

            _IUserRepository.Setup(x => x.DeleteUser(user))
                .Returns(true);
            
            var userController = new UserTestController(
                _IUserRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var userResult = userController.DeleteUser(user);

            // Assert
            Assert.True(userResult);
        }
    }
}
