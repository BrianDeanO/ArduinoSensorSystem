using System;
using Moq;
using Xunit;
using backEndApp.Models;
using backEndApp.Interfaces;
using backEndApp.TestControllers;
using backEndApp.UnitTests;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    public class TestController_UserDevice
    {
        private readonly Mock<IUserDeviceRepository> _IUserDeviceRepository;
        public List<UserDevice> userDeviceList = UnitTestHelper.GetUserDevices();
        public TestController_UserDevice() {
            _IUserDeviceRepository = new Mock<IUserDeviceRepository>();
        }

        [Fact]
        public void GetAllUserDevices() {
            // Arrange
            _IUserDeviceRepository.Setup(x => x.GetUserDevices())
                .Returns(userDeviceList);
            
            var userDeviceController = new UserDeviceTestController(
                _IUserDeviceRepository.Object
            );

            // Act
            var userDevicesResult = userDeviceController.GetUserDevices().ToArray();

            // Assert
            Assert.NotNull(userDevicesResult);
            Assert.Equal(userDeviceList[0].UserID, userDevicesResult[0].UserID);
            Assert.Equal(userDeviceList[0].DeviceID, userDevicesResult[0].DeviceID);
            Assert.Equal(userDeviceList[1].UserID, userDevicesResult[1].UserID);
            Assert.Equal(userDeviceList[1].DeviceID, userDevicesResult[1].DeviceID);
        }

        [Fact]
        public void GetUserDevicesByIDs() {
            // Arrange
            var userDevice = new UserDevice() {
                UserID = 1,
                DeviceID = 1
            };

            _IUserDeviceRepository.Setup(x => x.GetUserDevice(userDevice.UserID, userDevice.DeviceID))
                .Returns(userDevice);
            
            var userDeviceController = new UserDeviceTestController(
                _IUserDeviceRepository.Object
            );

            // Act
            var userDeviceResult = userDeviceController.GetUserDevice(userDevice.UserID, userDevice.DeviceID);

            // Assert
            Assert.NotNull(userDeviceResult);
            Assert.Equal(userDevice.UserID, userDeviceResult.UserID);
            Assert.Equal(userDevice.DeviceID, userDeviceResult.DeviceID);
        }

        [Fact]
        public void GetUserDevicesByUserID() {
            // Arrange
            var user = new User() {
                UserID = 1,
                UserType = "ADMIN", 
                UserFirstName = "Han",
                UserLastName = "Solo",
                UserPassword = "123"
            };

            _IUserDeviceRepository.Setup(x => x.GetUserDevices(user.UserID))
                .Returns(userDeviceList);
            
            var userDeviceController = new UserDeviceTestController(
                _IUserDeviceRepository.Object
            );

            // Act
            var userDeviceResult = userDeviceController.GetUserDevices(user.UserID).ToArray();

            // Assert
            Assert.NotNull(userDeviceResult);
            Assert.Equal(user.UserID, userDeviceResult[0].UserID);
            Assert.Equal(user.UserID, userDeviceResult[1].UserID);
        }

        [Fact]
        public void CreateUserDevice() {
            // Arrange
            var userDevice = new UserDevice() {
                UserID = 1,
                DeviceID = 1
            };

            // Setting up the User Devices
            _IUserDeviceRepository.Setup(x => x.CreateUserDevice(userDevice))
                .Returns(true);

            var userDeviceController = new UserDeviceTestController(
                _IUserDeviceRepository.Object
            );

            // Act
            var userDeviceResult = userDeviceController.CreateUserDevice(userDevice);

            // Assert
            Assert.NotNull(userDeviceResult);
            Assert.Equal(userDeviceResult.UserID, userDeviceResult.UserID);
            Assert.Equal(userDeviceResult.DeviceID, userDeviceResult.DeviceID);
        }

        [Fact]
        public void DeleteUserDevice() {
            // Arrange
            var userDevice = new UserDevice() {
                UserID = 1,
                DeviceID = 1
            };

            _IUserDeviceRepository.Setup(x => x.DeleteUserDevice(userDevice))
                .Returns(true);
            
            var userDeviceController = new UserDeviceTestController(
                _IUserDeviceRepository.Object
            );

            // Act
            var userDeviceResult = userDeviceController.DeleteUserDevice(userDevice);

            // Assert
            Assert.True(userDeviceResult);
        }
    }
}
