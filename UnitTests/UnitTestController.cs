using System;
using Moq;
using Xunit;
using backEndApp.DTO;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;
using backEndApp.Controllers;
using backEndApp.TestControllers;
using backEndApp.Repository;
using System.Collections.Generic;

namespace UnitTests
{
    public class UnitTestController
    {
        private readonly Mock<IDeviceRepository> _IDeviceRepository;

        public UnitTestController() {
            _IDeviceRepository = new Mock<IDeviceRepository>();
        }

        [Fact]
        public void GetAllDevices() {
            // Arrange
            var deviceList = GetDevices();
            _IDeviceRepository.Setup(x => x.GetDevices())
                .Returns(deviceList);
            
            var deviceController = new DeviceTestController(_IDeviceRepository.Object);

            // Act
            var devicesResult = deviceController.GetDevices();

            // Assert
            Assert.NotNull(devicesResult);
            Assert.Equal(deviceList[0].DeviceID, devicesResult[0].DeviceID);
            Assert.Equal(deviceList[1].DeviceID, devicesResult[1].DeviceID);
            Assert.Equal(deviceList[2].DeviceID, devicesResult[2].DeviceID);
        }

        [Fact]
        public void GetDeviceByID() {
            // Arrange
            var deviceList = GetDevices();
            _IDeviceRepository.Setup(x => x.GetDevice(1))
                .Returns(deviceList[0]);
            
            var deviceController = new DeviceTestController(_IDeviceRepository.Object);

            // Act
            var deviceResult = deviceController.GetDevice(1);

            Console.WriteLine("Device result ID - ", deviceResult.DeviceID);

            // Assert
            Assert.NotNull(deviceResult);
            Assert.Equal(deviceList[0].DeviceID, deviceResult.DeviceID);
            Assert.True(deviceList[0].DeviceIdent == deviceResult.DeviceIdent);
            Assert.True(deviceList[0].DeviceName == deviceResult.DeviceName);
        }

        [Fact]
        public void GetDeviceByIdent() {
            // Arrange
            var deviceList = GetDevices();
            _IDeviceRepository.Setup(x => x.GetDevice(1))
                .Returns(deviceList[0]);
            
            var deviceController = new DeviceTestController(_IDeviceRepository.Object);

            // Act
            var deviceResult = deviceController.GetDeviceIdent("ARD-123");

            // Assert
            Assert.NotNull(deviceResult);
            Assert.Equal(deviceList[0].DeviceID, deviceResult.DeviceID);
            Assert.True(deviceList[0].DeviceIdent == deviceResult.DeviceIdent);
            Assert.True(deviceList[0].DeviceName == deviceResult.DeviceName);
        }

        private List<Device> GetDevices() {
            List<Device> devices = new List<Device>() {
                new Device {
                        DeviceID = 1,
                        DeviceIdent = "ARD-123",
                        DeviceName = "PAWNEE_NW_1",
                        DeviceType = "ARDUINO",
                        DeviceZipCode = "98309",
                        DeviceUpdateInterval = 86400000,
                        DeviceLastSeen = DateTime.Parse("2024-03-10T11:42:27.069Z"),
                        DeviceIsDeleted = false,
                }, 
                new Device {
                        DeviceID = 2,
                        DeviceIdent = "ARD-456",
                        DeviceName = "LOTHAL_N_1",
                        DeviceType = "ARDUINO",
                        DeviceZipCode = "17715",
                        DeviceUpdateInterval = 86400000,
                        DeviceLastSeen = DateTime.Parse("2024-03-09T11:42:27.069Z"),
                        DeviceIsDeleted = false,
                },
                new Device {
                    DeviceID = 3,
                    DeviceIdent = "ARD-789",
                    DeviceName = "TIPOCA_E_1",
                    DeviceType = "ARDUINO",
                    DeviceZipCode = "88899",
                    DeviceUpdateInterval = 86400000,
                    DeviceLastSeen = DateTime.Parse("2024-03-10T10:42:27.069Z"),
                    DeviceIsDeleted = false,

                    Sensors = new List<Sensor> {
                        new Sensor() {
                            SensorID = 1,
                            SensorIdent = "SEN-323",
                            SensorName = "BME_4", 
                            SensorType = "Adafruit BME280", 
                            ChannelCount = 2,
                            SensorIsDeleted = false,
                        }
                    }
                }
            };
            return devices;
        }
    }
}
