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
    public class TestController_SensorConfig
    {
        private readonly Mock<ISensorConfigRepository> _ISensorConfigRepository;
        public List<SensorConfig> sensorConfigList = UnitTestHelper.GetSensorConfigs();

        public TestController_SensorConfig() {
            _ISensorConfigRepository = new Mock<ISensorConfigRepository>();
        }

        [Fact]
        public void GetAllSensorConfigs() {
            // Arrange
            _ISensorConfigRepository.Setup(x => x.GetSensorConfigs())
                .Returns(sensorConfigList);
            
            var sensorConfigController = new SensorConfigTestController(
                _ISensorConfigRepository.Object
            );

            // Act
            var sensorConfigResult = sensorConfigController.GetSensorConfigs().ToArray();

            // Assert
            Assert.NotNull(sensorConfigResult);
            Assert.Equal(sensorConfigList[0].SensorConfigID, sensorConfigResult[0].SensorConfigID);
            Assert.Equal(sensorConfigList[1].SensorConfigID, sensorConfigResult[1].SensorConfigID);
        }

        [Fact]
        public void GetAllSensorConfigsWithSensorID() {
            // Arrange
            var sensor = new Sensor() {
                SensorID = 1,
                SensorIdent = "SEN-123",
                SensorName = "BME_TRI_SENS"
            };
            _ISensorConfigRepository.Setup(x => x.GetSensorConfigs(sensor.SensorID))
                .Returns(sensorConfigList);
            
            var sensorConfigController = new SensorConfigTestController(
                _ISensorConfigRepository.Object
            );

            // Act
            var sensorConfigResult = sensorConfigController.GetSensorConfigs(sensor.SensorID).ToArray();

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
                _ISensorConfigRepository.Object
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
                _ISensorConfigRepository.Object
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
                _ISensorConfigRepository.Object
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
                _ISensorConfigRepository.Object
            );

            // Act
            var sensorResult = sensorConfigController.DeleteSensorConfig(sensorConfig);

            // Assert
            Assert.True(sensorResult);
        }
    }
}