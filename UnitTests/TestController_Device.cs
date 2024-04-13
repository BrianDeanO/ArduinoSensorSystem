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
    public class TestController_Device
    {
        private readonly Mock<IDeviceRepository> _IDeviceRepository;
        private readonly Mock<ISensorRepository> _ISensorRepository;
        public List<Device> deviceList = UnitTestHelper.GetDevices();

        public TestController_Device() {
            _IDeviceRepository = new Mock<IDeviceRepository>();
            _ISensorRepository = new Mock<ISensorRepository>();
        }

        [Fact]
        public void GetAllDevices() {
            // Arrange
            _IDeviceRepository.Setup(x => x.GetDevices())
                .Returns(deviceList);
            
            var deviceController = new DeviceTestController(
                _IDeviceRepository.Object
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
                _IDeviceRepository.Object
            );

            // Act
            var deviceResult = deviceController.GetDevice(device.DeviceID);

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
                _IDeviceRepository.Object
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

            // Setting up Device
            var device = new Device() {
                DeviceID = 1,
                DeviceIdent = "ARD-123",
                DeviceName = "PAWNEE_NW_1",
            };
            _IDeviceRepository.Setup(x => x.CreateDevice(device))
                .Returns(true);
            
            var deviceController = new DeviceTestController(
                _IDeviceRepository.Object
            );

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

            // Act
            var deviceResult = deviceController.CreateDevice(device);
            var sensorResult = sensorController.CreateSensor(sensor);

            // Asserting for Device Result
            Assert.NotNull(deviceResult);
            Assert.Equal(device.DeviceID, deviceResult.DeviceID);
            Assert.Equal(device.DeviceIdent, deviceResult.DeviceIdent);
            Assert.NotEqual(0, deviceResult.DeviceUpdateInterval);

            // Asserting for Sensor Result
            Assert.NotNull(sensorResult);
            Assert.Equal(sensor.SensorID, sensorResult.SensorID);
            Assert.Equal(sensor.SensorIdent, sensorResult.SensorIdent);
            Assert.False(sensorResult.SensorIsDeleted);
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
                _IDeviceRepository.Object
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
                _IDeviceRepository.Object
            );

            // Act
            var deviceResult = deviceController.DeleteDevice(device);

            // Assert
            Assert.True(deviceResult);
        }
    }
}