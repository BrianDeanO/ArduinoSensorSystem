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
    public class TestController_Sensor
    {
        private readonly Mock<ISensorRepository> _ISensorRepository;
        private readonly Mock<ISensorConfigRepository> _ISensorConfigRepository;
        public List<Sensor> sensorList = UnitTestHelper.GetSensors();
        public List<SensorConfig> sensorConfigList = UnitTestHelper.GetSensorConfigs();
        public List<SensorData> sensorDataList = UnitTestHelper.GetSensorData();
        public TestController_Sensor() {
            _ISensorRepository = new Mock<ISensorRepository>();
            _ISensorConfigRepository = new Mock<ISensorConfigRepository>();
        }

        [Fact]
        public void GetAllSensors() {
            // Arrange
            _ISensorRepository.Setup(x => x.GetSensors())
                .Returns(sensorList);
            
            var sensorController = new SensorTestController(
                _ISensorRepository.Object
            );
            // Act
            var sensorsResult = sensorController.GetSensors().ToArray();

            // Assert
            Assert.NotNull(sensorsResult);
            Assert.Equal(sensorList[0].SensorID, sensorsResult[0].SensorID);
            Assert.Equal(sensorList[1].SensorID, sensorsResult[1].SensorID);
        }

        [Fact]
        public void GetSensorByID() {
            // Arrange
            var sensor = new Sensor() {
                SensorID = 1,
                SensorIdent = "SEN-123",
                SensorName = "BME_TRI_SENS", 
                SensorType = "Adafruit BME280"
            };

            _ISensorRepository.Setup(x => x.GetSensor(sensor.SensorID))
                .Returns(sensor);
            
            var sensorController = new SensorTestController(
                _ISensorRepository.Object
            );

            // Act
            var sensorResult = sensorController.GetSensor(sensor.SensorID);

            // Assert
            Assert.NotNull(sensorResult);
            Assert.Equal(sensor.SensorID, sensorResult.SensorID);
        }

        [Fact]
        public void GetSensorDatas() {
            // Arrange
            var sensor = new Sensor() {
                SensorID = 1,
                SensorIdent = "SEN-123",
                SensorName = "BME_TRI_SENS", 
                SensorType = "Adafruit BME280"
            };

            _ISensorRepository.Setup(x => x.GetSensorDatas(sensor.SensorID))
                .Returns(sensorDataList);
            
            var sensorController = new SensorTestController(
                _ISensorRepository.Object
            );

            // Act
            var sensorDatasResult = sensorController.GetSensorDatas(sensor.SensorID).ToArray();

            // Assert
            Assert.NotNull(sensorDatasResult);
            Assert.Equal(sensor.SensorID, sensorDatasResult[0].SensorID);
            Assert.Equal(sensor.SensorID, sensorDatasResult[1].SensorID);
            Assert.Equal(sensor.SensorID, sensorDatasResult[2].SensorID);
        }

        [Fact]
        public void GetSensorConfigs() {
            // Arrange
            var sensor = new Sensor() {
                SensorID = 1,
                SensorIdent = "SEN-123",
                SensorName = "BME_TRI_SENS", 
                SensorType = "Adafruit BME280"
            };

            _ISensorRepository.Setup(x => x.GetSensorConfigs(sensor.SensorID))
                .Returns(sensorConfigList);
            
            var sensorController = new SensorTestController(
                _ISensorRepository.Object
            );

            // Act
            var sensorConfigResult = sensorController.GetSensorConfigs(sensor.SensorID).ToArray();

            // Assert
            Assert.NotNull(sensorConfigResult);
            Assert.Equal(sensor.SensorID, sensorConfigResult[0].SensorID);
            Assert.Equal(sensor.SensorID, sensorConfigResult[1].SensorID);
            Assert.Equal("Gain", sensorConfigResult[0].SensorConfigKey);
            Assert.Equal("17.3", sensorConfigResult[0].SensorConfigValue);
        }
        [Fact]
        public void CreateSensor() {
            // Arrange
            var sensorConfigCreationSuccess = true;

            // Setting up Sensor
            var sensor = new Sensor() {
                SensorID = 1,
                SensorIdent = "SEN-123",
                SensorName = "BME_TRI_SENS"
            };

            _ISensorRepository.Setup(x => x.CreateSensor(sensor))
                .Returns(true);            
            
            var sensorController = new SensorTestController(
                _ISensorRepository.Object
            );


            // Setting up for Sensor Config Creation
            _ISensorConfigRepository.Setup(x => x.CreateSensorConfig(sensorConfigList[0]))
                .Returns(true);

            var sensorConfigController = new SensorConfigTestController(
                _ISensorConfigRepository.Object
            );

            // Act
            var sensorResult = sensorController.CreateSensor(sensor);

            // Create New Sensor Configs
            foreach (var sensorConfig in sensorConfigList) {
                var sensorConfigResult = sensorConfigController.CreateSensorConfig(sensorConfig);
                if(sensorConfigResult == null) {
                    sensorConfigCreationSuccess = false;
                }
            }

            // Assert
            Assert.NotNull(sensorResult);
            Assert.True(sensorConfigCreationSuccess);
            Assert.Equal(sensor.SensorID, sensorResult.SensorID);
            Assert.Equal(sensor.SensorIdent, sensorResult.SensorIdent);
            Assert.False(sensorResult.SensorIsDeleted);
        }

        [Fact]
        public void UpdateSensor() {
            // Arrange
            var sensor = new Sensor() {
                SensorID = 1,
                SensorIdent = "SEN-123",
                SensorName = "BME_TRI_SENS"
            };
            var newSensorName = "NEW_SENSOR_NAME";

            _ISensorRepository.Setup(x => x.UpdateSensor(sensor))
                .Returns(true);
            
            var sensorController = new SensorTestController(
                _ISensorRepository.Object
            );

            // Act
            var sensorResult = sensorController.UpdateSensor(sensor, newSensorName);

            // Assert
            Assert.NotNull(sensorResult);
            Assert.Equal(sensor.SensorID, sensorResult.SensorID);
            Assert.Equal(newSensorName, sensorResult.SensorName);
        }

        [Fact]
        public void DeleteSensor() {
            // Arrange
            var sensor = new Sensor() {
                SensorID = 1,
                SensorIdent = "SEN-123",
                SensorName = "BME_TRI_SENS"
            };

            _ISensorRepository.Setup(x => x.DeleteSensor(sensor))
                .Returns(true);
            
            var sensorController = new SensorTestController(
                _ISensorRepository.Object
            );

            // Act
            var sensorResult = sensorController.DeleteSensor(sensor);

            // Assert
            Assert.True(sensorResult);
        }
    }
}
