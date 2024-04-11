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
    public class TestController_SensorConfig
    {
        private readonly Mock<ISensorConfigRepository> _ISensorConfigRepository;
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

        public TestController_SensorConfig() {
            _ISensorConfigRepository = new Mock<ISensorConfigRepository>();
            // _mapper = new IMapper();
            _userDeviceRepository = new Mock<IUserDeviceRepository>();
            _ISensorRepository = new Mock<ISensorRepository>();
        }

        [Fact]
        public void GetAllSensorConfigs() {
            // Arrange
            _ISensorConfigRepository.Setup(x => x.GetSensorConfigs())
                .Returns(sensorConfigList);
            
            var sensorConfigController = new SensorConfigTestController(
                _ISensorConfigRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var sensorConfigResult = sensorConfigController.GetSensorConfigs().ToArray();

            // Assert
            Assert.NotNull(sensorConfigResult);
            Assert.Equal(sensorConfigList[0].SensorConfigID, sensorConfigResult[0].SensorConfigID);
            Assert.Equal(sensorConfigList[1].SensorConfigID, sensorConfigResult[1].SensorConfigID);
        }

        [Fact]
        public void GetSensorConfigByID() {
            // Arrange
            var sensorConfig = new SensorConfig() {
                SensorConfigID = 1,
                SensorID = 1,
                SensorConfigKey = "Gain",
                SensorConfigValue = "17.3",
            };

            _ISensorConfigRepository.Setup(x => x.GetSensorConfig(sensorConfig.SensorConfigID))
                .Returns(sensorConfig);
            
            var sensorConfigController = new SensorConfigTestController(
                _ISensorConfigRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var sensorConfigResult = sensorConfigController.GetSensorConfig(sensorConfig.SensorConfigID);

            // Assert
            Assert.NotNull(sensorConfigResult);
            Assert.Equal(sensorConfig.SensorConfigID, sensorConfigResult.SensorConfigID);
            Assert.True(sensorConfig.SensorConfigKey == sensorConfigResult.SensorConfigKey);
            Assert.True(sensorConfig.SensorConfigValue == sensorConfigResult.SensorConfigValue);
        }

        [Fact]
        public void CreateSensorConfig() {
            // Arrange
            var sensorConfig = new SensorConfig() {
                SensorConfigID = 1,
                SensorID = 1,
                SensorConfigKey = "Gain",
                SensorConfigValue = "17.3",
            };

            _ISensorConfigRepository.Setup(x => x.CreateSensorConfig(sensorConfig))
                .Returns(true);
            
            var sensorConfigController = new SensorConfigTestController(
                _ISensorConfigRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var sensorConfigResult = sensorConfigController.CreateSensorConfig(sensorConfig);

            // Assert
            Assert.NotNull(sensorConfigResult);
            Assert.Equal(sensorConfig.SensorConfigID, sensorConfigResult.SensorConfigID);
            Assert.True(sensorConfig.SensorConfigKey == sensorConfigResult.SensorConfigKey);
            Assert.True(sensorConfig.SensorConfigValue == sensorConfigResult.SensorConfigValue);
        }

        [Fact]
        public void UpdateSensorConfig() {
            // Arrange
            var sensorConfig = new SensorConfig() {
                SensorConfigID = 1,
                SensorID = 1,
                SensorConfigKey = "Gain",
                SensorConfigValue = "17.3",
            };
            var newConfigValue = "42.19";

            _ISensorConfigRepository.Setup(x => x.UpdateSensorConfig(sensorConfig))
                .Returns(true);
            
            var sensorConfigController = new SensorConfigTestController(
                _ISensorConfigRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var sensorConfigResult = sensorConfigController.UpdateSensorConfig(sensorConfig, newConfigValue);

            // Assert
            Assert.NotNull(sensorConfigResult);
            Assert.Equal(sensorConfig.SensorConfigID, sensorConfigResult.SensorConfigID);
            Assert.Equal(newConfigValue, sensorConfigResult.SensorConfigValue);
        }

        [Fact]
        public void DeleteSensorConfig() {
            // Arrange
            var sensorConfig = new SensorConfig() {
                SensorConfigID = 1,
                SensorID = 1,
                SensorConfigKey = "Gain",
                SensorConfigValue = "17.3",
            };

            _ISensorConfigRepository.Setup(x => x.DeleteSensorConfig(sensorConfig))
                .Returns(true);
            
            var sensorConfigController = new SensorConfigTestController(
                _ISensorConfigRepository.Object, 
                deviceList,
                userList,
                sensorList,
                sensorDataList,
                sensorConfigList,
                userDeviceList
            );

            // Act
            var sensorResult = sensorConfigController.DeleteSensorConfig(sensorConfig);

            // Assert
            Assert.True(sensorResult);
        }
    }
}