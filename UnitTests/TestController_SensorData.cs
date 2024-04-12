using System;
using Moq;
using Xunit;
using backEndApp.Models;
using backEndApp.Interfaces;
using backEndApp.TestControllers;
using backEndApp.UnitTests;
using backEndApp.Repository;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests
{
    public class TestController_SensorData
    {
        private readonly Mock<ISensorDataRepository> _ISensorDataRepository;
        public List<SensorData> sensorDataList = UnitTestHelper.GetSensorData();

        public TestController_SensorData() {
            _ISensorDataRepository = new Mock<ISensorDataRepository>();
        }

        [Fact]
        public void GetAllSensorData() {
            // Arrange
            _ISensorDataRepository.Setup(x => x.GetSensorDatas())
                .Returns(sensorDataList);
            
            var sensorDataController = new SensorDataTestController(
                _ISensorDataRepository.Object
            );

            // Act
            var sensorDatasResult = sensorDataController.GetSensorDatas().ToArray();

            // Assert
            Assert.NotNull(sensorDatasResult);
            Assert.Equal(sensorDataList[0].SensorDataID, sensorDatasResult[0].SensorDataID);
            Assert.Equal(sensorDataList[0].SensorID, sensorDatasResult[0].SensorID);
            Assert.Equal(sensorDataList[0].DataValue, sensorDatasResult[0].DataValue);
            Assert.Equal(sensorDataList[1].SensorDataID, sensorDatasResult[1].SensorDataID);
            Assert.Equal(sensorDataList[1].SensorID, sensorDatasResult[1].SensorID);
            Assert.Equal(sensorDataList[1].DataValue, sensorDatasResult[1].DataValue);
        }

        [Fact]
        public void GetSensorDataByDataID() {
            // Arrange
            var sensorData = new SensorData() {
                SensorDataID = 1,
                SensorID = 1,
                ChannelID = 0, 
                DataValue = 44,
                DataUnit = "F", 
                TimeRecorded = DateTime.Parse("2024-03-11T18:42:27.069Z")
            };

            _ISensorDataRepository.Setup(x => x.GetSensorData(sensorData.SensorDataID))
                .Returns(sensorData);
            
            var sensorDataController = new SensorDataTestController(
                _ISensorDataRepository.Object
            );

            // Act
            var sensorDataResult = sensorDataController.GetSensorData(sensorData.SensorDataID);

            // Assert
            Assert.NotNull(sensorDataResult);
            Assert.Equal(sensorData.SensorDataID, sensorDataResult.SensorDataID);
            Assert.Equal(sensorData.SensorID, sensorDataResult.SensorID);
            Assert.Equal(sensorData.DataValue, sensorDataResult.DataValue);
        }

        
        [Fact]
        public void CreateSensorData() {
            // Arrange
            var sensorData = new SensorData() {
                SensorDataID = 1,
                SensorID = 1,
                ChannelID = 0, 
                DataValue = 44,
                DataUnit = "F", 
                TimeRecorded = DateTime.Parse("2024-03-11T18:42:27.069Z")
            };

            _ISensorDataRepository.Setup(x => x.CreateSensorData(sensorData))
                .Returns(true);
            
            var sensorDataController = new SensorDataTestController(
                _ISensorDataRepository.Object
            );

            // Act
            var sensorDataResult = sensorDataController.CreateSensorData(sensorData);

            // Assert
            Assert.NotNull(sensorDataResult);
            Assert.Equal(sensorData.SensorDataID, sensorDataResult.SensorDataID);
            Assert.Equal(sensorData.SensorID, sensorDataResult.SensorID);
            Assert.Equal(sensorData.DataValue, sensorDataResult.DataValue);
        }

        [Fact]
        public void UpdateSensorData() {
            // Arrange
            var sensorData = new SensorData() {
                SensorDataID = 1,
                SensorID = 1,
                ChannelID = 0, 
                DataValue = 44,
                DataUnit = "F", 
                TimeRecorded = DateTime.Parse("2024-03-11T18:42:27.069Z")
            };
            string newDataUnit = "C";

            _ISensorDataRepository.Setup(x => x.UpdateSensorData(sensorData))
                .Returns(true);
            
            var sensorDataController = new SensorDataTestController(
                _ISensorDataRepository.Object
            );

            // Act
            var sensorDataResult = sensorDataController.UpdateSensorData(sensorData, newDataUnit);

            // Assert
            Assert.NotNull(sensorDataResult);
            Assert.Equal(sensorData.SensorDataID, sensorDataResult.SensorDataID);
            Assert.Equal(newDataUnit, sensorDataResult.DataUnit);
        }

        [Fact]
        public void DeleteSensorConfig() {
            // Arrange
            var sensorData = new SensorData() {
                SensorDataID = 1,
                SensorID = 1,
                ChannelID = 0, 
                DataValue = 44,
                DataUnit = "F", 
                TimeRecorded = DateTime.Parse("2024-03-11T18:42:27.069Z")
            };

            _ISensorDataRepository.Setup(x => x.DeleteSensorData(sensorData))
                .Returns(true);
            
            var sensorDataController = new SensorDataTestController(
                _ISensorDataRepository.Object
            );

            // Act
            var sensorDataResult = sensorDataController.DeleteSensorData(sensorData);

            // Assert
            Assert.True(sensorDataResult);
        }
    }
}