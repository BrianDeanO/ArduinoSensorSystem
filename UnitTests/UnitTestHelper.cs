using System;
using System.Collections.Generic;
using backEndApp.Models;
using backEndApp.Data;

namespace backEndApp.UnitTests {
    public class UnitTestHelper {
        public UnitTestHelper() {
        }

        public static List<User> GetUsers() {
            var Users = new List<User>() {
                new User() {
                    UserType = "ADMIN", 
                    UserFirstName = "Han",
                    UserLastName = "Solo",
                    UserPassword = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",  // 123
                    UserEmail = "Han.Solo@gmail.com",
                    UserPhone = "5091234567",
                    UserNotifications = true,
                    UserIsDeleted = false
                },
                new User() {
                    UserType = "BASIC", 
                    UserFirstName = "Luke",
                    UserLastName = "Skywalker",
                    UserPassword = "b3a8e0e1f9ab1bfe3a36f231f676f78bb30a519d2b21e6c530c0eee8ebb4a5d0", // 456
                    UserEmail = "Luke.Skywalker@gmail.com",
                    UserPhone = "5094734201",
                    UserNotifications = false,
                    UserIsDeleted = false
                },
                new User() {
                    UserType = "BASIC", 
                    UserFirstName = "Leia",
                    UserLastName = "Organa",
                    UserPassword = "35a9e381b1a27567549b5f8a6f783c167ebf809f1c4d6a9e367240484d8ce281", // 789
                    UserEmail = "Leia.Organa@gmail.com",
                    UserPhone = "5099814556",
                    UserNotifications = true,
                    UserIsDeleted = false
                },
            };
            return Users;
        }
        public static List<Device> GetDevices() {
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
                }
            };
            return devices;
        }

        public static List<Sensor> GetSensors() {
            List<Sensor> Sensors = new List<Sensor>() {
                new Sensor() {
                    SensorID = 1,
                    SensorIdent = "SEN-123",
                    SensorName = "BME_TRI_SENS", 
                    SensorType = "Adafruit BME280", 
                    SensorIsDeleted = false,
                    ChannelCount = 3,
                    DeviceID = 1
                },
                new Sensor() {
                    SensorID = 2,
                    SensorIdent = "SEN-456",
                    SensorName = "BME_2", 
                    SensorType = "Adafruit BME280", 
                    ChannelCount = 1,
                    DeviceID = 1,
                    SensorIsDeleted = false
                }
            };
            return Sensors;
        }
        
        public static List<SensorConfig> GetSensorConfigs() {
            List<SensorConfig> sensorConfigs = new List<SensorConfig>() {
                new SensorConfig() {
                    SensorConfigID = 1,
                    SensorID = 1,
                    SensorConfigKey = "Gain",
                    SensorConfigValue = "17.3",
                },
                new SensorConfig() {
                    SensorConfigID = 2,
                    SensorID = 1,
                    SensorConfigKey = "Offset",
                    SensorConfigValue = "22",
                }
            };
            return sensorConfigs;
        }

        public static List<SensorData> GetSensorData() {
            List<SensorData> sensorDatas = new List<SensorData>() {
                new SensorData() {
                    SensorDataID = 1,
                    SensorID = 1,
                    ChannelID = 0, 
                    DataValue = 44,
                    DataUnit = "F", 
                    TimeRecorded = DateTime.Parse("2024-03-11T18:42:27.069Z")
                },
                new SensorData() {
                    SensorDataID = 2,
                    SensorID = 1,
                    ChannelID = 0, 
                    DataValue = 52, 
                    DataUnit = "F", 
                    TimeRecorded = DateTime.Parse("2024-03-12T18:42:27.069Z")
                },
                new SensorData() {
                    SensorDataID = 3,
                    SensorID = 1,
                    ChannelID = 0, 
                    DataValue = 51, 
                    DataUnit = "F", 
                    TimeRecorded = DateTime.Parse("2024-03-13T18:42:27.069Z")
                }
            };
            return sensorDatas;
        }

        public static List<UserDevice> GetUserDevices() {
            List<UserDevice> userDevices = new List<UserDevice>() {
                new UserDevice() {
                    UserID = 1,
                    DeviceID = 1
                },
                new UserDevice() {
                    UserID = 1,
                    DeviceID = 2
                },
                new UserDevice() {
                    UserID = 2,
                    DeviceID = 2
                }
            };
            return userDevices;
        }
    }
}