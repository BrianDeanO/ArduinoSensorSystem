using System;
using AutoMapper;
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
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace UnitTests
{
    public class TestController_Device
    {
        private readonly Mock<IDeviceRepository> _IDeviceRepository;
        private readonly IMapper _mapper;
        private readonly Mock<IUserDeviceRepository> _userDeviceRepository;
        private readonly Mock<ISensorRepository> _ISensorRepository;
        private readonly UserRepository _userRepository;
        public List<Device> deviceList = UnitTestHelper.GetDevices(); 
        public List<User> userList = UnitTestHelper.GetUsers();
        public List<Sensor> sensorList = UnitTestHelper.GetSensors();
        public List<SensorData> sensorDataList = UnitTestHelper.GetSensorData();
        public List<SensorConfig> sensorConfigList = UnitTestHelper.GetSensorConfigs();
        public List<UserDevice> userDeviceList = UnitTestHelper.GetUserDevices();

        public TestController_Device() {
            _IDeviceRepository = new Mock<IDeviceRepository>();
        }

        [Fact]
        public void GetAllDevices() {
            // Arrange
            _IDeviceRepository.Setup(x => x.GetDevices())
                .Returns(deviceList);
            
            var deviceController = new DeviceTestController(
                _IDeviceRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var devicesResult = deviceController.GetDevices().ToArray();

            // Assert
            Assert.NotNull(devicesResult);
            Assert.Equal(deviceList[0].DeviceID, devicesResult[0].DeviceID);
            Assert.Equal(deviceList[1].DeviceID, devicesResult[1].DeviceID);
        }

        [Fact]
        public void GetDeviceByID() {
            // Arrange
            var device = new Device() {
                DeviceID = 1,
                DeviceIdent = "ARD-123",
                DeviceName = "PAWNEE_NW_1",
            };

            _IDeviceRepository.Setup(x => x.GetDevice(device.DeviceID))
                .Returns(device);
            
            var deviceController = new DeviceTestController(
                _IDeviceRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var deviceResult = deviceController.GetDevice(device.DeviceID);

            Console.WriteLine("Device result ID - ", deviceResult.DeviceID);

            // Assert
            Assert.NotNull(deviceResult);
            Assert.Equal(device.DeviceID, deviceResult.DeviceID);
            Assert.True(device.DeviceIdent == deviceResult.DeviceIdent);
            Assert.True(device.DeviceName == deviceResult.DeviceName);
        }

        [Fact]
        public void GetDeviceConfigInfo() {
            // Arrange
            var device = new Device() {
                DeviceID = 1,
                DeviceIdent = "ARD-123",
                DeviceName = "PAWNEE_NW_1",
                DeviceUpdateInterval = 86400000,
            };
            _IDeviceRepository.Setup(x => x.GetDevice(device.DeviceID))
                .Returns(device);
            
            var deviceController = new DeviceTestController(
                _IDeviceRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var deviceResult = deviceController.GetDeviceConfigInfo(device.DeviceID);

            // Assert
            Assert.NotNull(deviceResult);
            Assert.Equal(device.DeviceUpdateInterval, deviceResult.DeviceUpdateInterval);
        }

        [Fact]
        public void CreateDevice() {
            // Arrange
            var device = new Device() {
                DeviceID = 1,
                DeviceIdent = "ARD-123",
                DeviceName = "PAWNEE_NW_1",
            };
            _IDeviceRepository.Setup(x => x.CreateDevice(device))
                .Returns(true);
            
            var deviceController = new DeviceTestController(
                _IDeviceRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var deviceResult = deviceController.CreateDevice(device);

            // Assert
            Assert.NotNull(deviceResult);
            Assert.Equal(device.DeviceID, deviceResult.DeviceID);
            Assert.Equal(device.DeviceIdent, deviceResult.DeviceIdent);
            Assert.NotEqual(0, deviceResult.DeviceUpdateInterval);
        }

        [Fact]
        public void UpdateDevice() {
            // Arrange
            var device = new Device() {
                DeviceID = 1,
                DeviceIdent = "ARD-123",
                DeviceName = "PAWNEE_NW_1",
            };
            var newDeviceName = "BATUU_NW_1";

            _IDeviceRepository.Setup(x => x.UpdateDevice(device))
                .Returns(true);
            
            var deviceController = new DeviceTestController(
                _IDeviceRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var deviceResult = deviceController.UpdateDevice(device, newDeviceName);

            // Assert
            Assert.NotNull(deviceResult);
            Assert.Equal(device.DeviceID, deviceResult.DeviceID);
            Assert.Equal(newDeviceName, deviceResult.DeviceName);
        }

        [Fact]
        public void DeleteDevice() {
            // Arrange
            var device = new Device() {
                DeviceID = 1,
                DeviceIdent = "ARD-123",
                DeviceName = "PAWNEE_NW_1",
            };

            _IDeviceRepository.Setup(x => x.DeleteDevice(device))
                .Returns(true);
            
            var deviceController = new DeviceTestController(
                _IDeviceRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var deviceResult = deviceController.DeleteDevice(device);

            // Assert
            Assert.True(deviceResult);
        }
        // [Fact]
        // public void GetDeviceSensors() {
        //     // Arrange
        //     var device = new Device() {
        //         DeviceID = 1,
        //         DeviceIdent = "ARD-123",
        //         DeviceName = "PAWNEE_NW_1",
        //         DeviceUpdateInterval = 86400000,
        //     };

        //     var deviceController = new DeviceTestController(
        //         _IDeviceRepository.Object,
        //         deviceList,
        //         userList,
        //         sensorList,
        //         sensorDataList,
        //         sensorConfigList,
        //         userDeviceList
        //     );

        //     // Act
        //     var sensorsResult = deviceController.GetDeviceSensors();

        //     // Assert
        //     Assert.NotNull(sensorsResult);
        //     Assert.Equal(sensorList[0].SensorID, sensorsResult[0].SensorID);
        //     Assert.Equal(sensorList[1].SensorID, sensorsResult[1].SensorID);
        //     Assert.Equal(device.DeviceID, sensorsResult[0].DeviceID);
        //     Assert.Equal(device.DeviceID, sensorsResult[1].DeviceID);
        // }

        // [Fact]
        // public void GetUserDevices() {
        //     // Arrange

        //     var user = new User() {
        //         UserID = 1,
        //         UserType = "ADMIN", 
        //         UserFirstName = "Han",
        //         UserLastName = "Solo",
        //         UserPassword = "123"
        //     };
            

            
        //     var deviceController = new DeviceTestController(_IDeviceRepository.Object);

        //     // Act
        //     var userDeviceResult = deviceController.GetUserDevices();

        //     // Assert
        //     Assert.NotNull(userDeviceResult);
        //     Assert.Equal(userDeviceList[0].UserID, user.UserID);
        //     Assert.Equal(userDeviceList[0].DeviceID, deviceList[0].DeviceID);
        //     Assert.Equal(userDeviceList[1].UserID, user.UserID);
        //     Assert.Equal(userDeviceList[1].DeviceID, deviceList[1].DeviceID);

        //     Assert.Equal(userDeviceList[0].UserID, userDeviceResult[0].UserID);
        //     Assert.Equal(userDeviceList[0].DeviceID, userDeviceResult[0].DeviceID);
        //     Assert.Equal(userDeviceList[1].UserID, userDeviceResult[1].UserID);
        //     Assert.Equal(userDeviceList[1].DeviceID, userDeviceResult[1].DeviceID);
        // }

        // [Fact]
        // public void GetUsersFromDevice() {
        //     // Arrange
        //     var device = new Device() {
        //         DeviceID = 1,
        //         DeviceIdent = "ARD-123",
        //         DeviceName = "PAWNEE_NW_1",
        //         DeviceUpdateInterval = 86400000,
        //     };
            
        //     var deviceController = new DeviceTestController(_IDeviceRepository.Object);

        //     // Act
        //     var usersFromDeviceResult = deviceController.GetUsersFromDevice();

        //     // Assert
        //     Assert.NotNull(usersFromDeviceResult);

        //     Assert.Equal(userDeviceList[0].UserID, usersFromDeviceResult[0].UserID);
        //     Assert.Equal(userDeviceList[0].UserID, usersFromDeviceResult[0].UserID);
        //     Assert.Equal(userDeviceList[1].UserID, usersFromDeviceResult[1].UserID);
        //     Assert.Equal(userDeviceList[1].UserID, usersFromDeviceResult[1].UserID);
        // }
    }


}

        // [Fact]
        // public void GetDeviceByIdent() {
        //     // Arrange
        //     var device = new Device() {
        //         DeviceID = 1,
        //         DeviceIdent = "ARD-123",
        //         DeviceName = "PAWNEE_NW_1",
        //     };
            
        //     var deviceController = new DeviceTestController(
        //         _IDeviceRepository.Object, 
        //         deviceList,
        //         userList,
        //         sensorList,
        //         sensorDataList,
        //         sensorConfigList,
        //         userDeviceList
        //     );

        //     // Act
        //     var deviceResult = deviceController.GetDeviceIdent(device.DeviceIdent);

        //     // Assert
        //     Assert.NotNull(deviceResult);
        //     Assert.Equal(device.DeviceID, deviceResult.DeviceID);
        //     Assert.True(device.DeviceIdent == deviceResult.DeviceIdent);
        //     Assert.True(device.DeviceName == deviceResult.DeviceName);
        // }
