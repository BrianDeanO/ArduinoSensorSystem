using Microsoft.AspNetCore.Identity;
using backEndApp.Models;
using backEndApp.Data;

namespace backEndApp
{
    public class Seed {
        private readonly SensorSystemContext sensorSystemContext;
        public Seed(SensorSystemContext context) {
            this.sensorSystemContext = context;
        }
        public void SeedSensorSystemContext()
        {
            if (!sensorSystemContext.UserDevices.Any()) {
                var Users = new List<User>() {
                    // Han is our Default Admin user
                    new User() {
                        UserType = "ADMIN", 
                        UserFirstName = "Han",
                        UserLastName = "Solo",
                        UserPassword = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",  // Password = 123
                        UserEmail = "Han.Solo@gmail.com",
                        UserPhone = "5091234567",
                        UserNotifications = true,
                        UserIsDeleted = false
                    },
                    new User() {
                        UserType = "BASIC", 
                        UserFirstName = "Luke",
                        UserLastName = "Skywalker",
                        UserPassword = "b3a8e0e1f9ab1bfe3a36f231f676f78bb30a519d2b21e6c530c0eee8ebb4a5d0", // Password = 456
                        UserEmail = "Luke.Skywalker@gmail.com",
                        UserPhone = "5094734201",
                        UserNotifications = false,
                        UserIsDeleted = false
                    },
                    new User() {
                        UserType = "BASIC", 
                        UserFirstName = "Leia",
                        UserLastName = "Organa",
                        UserPassword = "35a9e381b1a27567549b5f8a6f783c167ebf809f1c4d6a9e367240484d8ce281", // Password = 789
                        UserEmail = "Leia.Organa@gmail.com",
                        UserPhone = "5099814556",
                        UserNotifications = true,
                        UserIsDeleted = false
                    },
                };
                var Devices = new List<Device>() {
                    new Device() {
                        DeviceIdent = "ARD-123",
                        DeviceName = "RICHLAND_NW_1",
                        DeviceType = "ARDUINO",
                        DeviceZipCode = "99352",
                        DeviceUpdateInterval = 86400000,
                        DeviceLastSeen = DateTime.Parse("2024-03-30T11:42:27.069Z"),
                        DeviceIsDeleted = false,

                        Sensors = new List<Sensor>() {
                            new Sensor() {
                                SensorIdent = "SEN-123",
                                SensorName = "BME_TRI_SENS", 
                                SensorType = "Adafruit BME280", 
                                SensorIsDeleted = false,
                                ChannelCount = 3,
                                DeviceID = 1,
                                SensorConfigs = new List<SensorConfig> () {
                                    new SensorConfig() {
                                        SensorConfigKey = "Gain",
                                        SensorConfigValue = "17.3",
                                    },
                                    new SensorConfig() {
                                        SensorConfigKey = "Offset",
                                        SensorConfigValue = "22",
                                    }
                                },
                                SensorDatas = new List<SensorData>() {
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 44,
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-11T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 52, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-12T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 51, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-13T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 57, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-14T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 62, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-15T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 61, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-17T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 56, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-21T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 57, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-22T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 58, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-23T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 68, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-24T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 46, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-25T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 49, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-26T18:42:27.069Z")
                                    },
                                    
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 1200, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-03-16T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1,
                                        DataValue = 1310, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-03-11T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 1399, 
                                        DataUnit = "W/m^2",  
                                        TimeRecorded = DateTime.Parse("2024-03-12T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 1378, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-03-13T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 1290,
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-03-14T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 1368, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-03-15T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 1340, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-03-17T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1,
                                        DataValue = 1300, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-03-21T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 1361, 
                                        DataUnit = "W/m^2",  
                                        TimeRecorded = DateTime.Parse("2024-03-22T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 1411, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-03-23T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 1391, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-03-24T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 1390, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-03-25T18:42:27.069Z")
                                    },

                                    new SensorData() {
                                        ChannelID = 2,
                                        DataValue = 30,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-03-11T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 26,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-03-12T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 28,
                                        DataUnit = "atm", 
                                        TimeRecorded = DateTime.Parse("2024-03-13T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 27,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-03-14T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 35,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-03-15T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 23,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-03-17T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2,
                                        DataValue = 30,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-03-21T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 25,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-03-22T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 24,
                                        DataUnit = "atm", 
                                        TimeRecorded = DateTime.Parse("2024-03-23T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 31,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-03-24T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 32,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-03-25T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 29,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-03-26T18:42:27.069Z")
                                    }
                                }
                            },
                            new Sensor() {
                                SensorIdent = "SEN-456",
                                SensorName = "BME_2", 
                                SensorType = "Adafruit BME280", 
                                ChannelCount = 1,
                                DeviceID = 1,
                                SensorIsDeleted = false,
                                SensorConfigs = new List<SensorConfig> () {
                                    new SensorConfig() {
                                        SensorConfigKey = "Gain",
                                        SensorConfigValue = "9",
                                    },
                                    new SensorConfig() {
                                        SensorConfigKey = "Offset",
                                        SensorConfigValue = "2.4",
                                    }
                                },
                                SensorDatas = new List<SensorData>() {
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 1361, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-03-17T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 1459,
                                        DataUnit = "W/m^2",
                                        TimeRecorded = DateTime.Parse("2024-03-18T18:42:27.069Z")
                                    },
                                }
                            }
                        }
                    },
                    new Device() {
                        DeviceIdent = "ARD-456",
                        DeviceName = "KENNEWICK_N_1",
                        DeviceType = "ARDUINO",
                        DeviceZipCode = "99336",
                        DeviceUpdateInterval = 86400000,
                        DeviceLastSeen = DateTime.Parse("2024-03-29T11:42:27.069Z"),
                        DeviceIsDeleted = false,
                        
                        Sensors = new List<Sensor>() {
                            new Sensor() {
                                SensorIdent = "SEN-789",
                                SensorName = "BME_3", 
                                SensorType = "Adafruit BME280", 
                                ChannelCount = 2,
                                DeviceID = 2,
                                SensorIsDeleted = false,
                                SensorConfigs = new List<SensorConfig> () {
                                    new SensorConfig() {
                                        SensorConfigKey = "Gain",
                                        SensorConfigValue = "1.3",
                                    },
                                    new SensorConfig() {
                                        SensorConfigKey = "Offset",
                                        SensorConfigValue = "0",
                                    }
                                },
                                SensorDatas = new List<SensorData>() {
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 30,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-01-09T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 31,
                                        DataUnit = "atm", 
                                        TimeRecorded = DateTime.Parse("2024-01-10T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 33,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-01-11T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 29,
                                        DataUnit = "atm", 
                                        TimeRecorded = DateTime.Parse("2024-01-12T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 31, 
                                        DataUnit = "atm", 
                                        TimeRecorded = DateTime.Parse("2024-01-13T18:42:27.069Z")
                                    },
                                }
                            },
                            new Sensor() {
                                SensorIdent = "SEN-223",
                                SensorName = "BME_4", 
                                SensorType = "Adafruit BME280", 
                                ChannelCount = 2,
                                DeviceID = 2,
                                SensorIsDeleted = false,
                                SensorDatas = new List<SensorData>() {
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 30,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-01-09T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 31,
                                        DataUnit = "atm", 
                                        TimeRecorded = DateTime.Parse("2024-01-10T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 33,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-01-11T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 29,
                                        DataUnit = "atm", 
                                        TimeRecorded = DateTime.Parse("2024-01-12T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 0, 
                                        DataValue = 31, 
                                        DataUnit = "atm", 
                                        TimeRecorded = DateTime.Parse("2024-01-13T18:42:27.069Z")
                                    },
                                }
                            }
                        }
                    },
                    new Device() {
                        DeviceIdent = "ARD-789",
                        DeviceName = "PASCO_E_1",
                        DeviceType = "ARDUINO",
                        DeviceZipCode = "99301",
                        DeviceUpdateInterval = 86400000,
                        DeviceLastSeen = DateTime.Parse("2024-03-10T10:42:27.069Z"),
                        DeviceIsDeleted = false,

                        Sensors = new List<Sensor>() {
                            new Sensor() {
                                SensorIdent = "SEN-323",
                                SensorName = "BME_4", 
                                SensorType = "Adafruit BME280", 
                                ChannelCount = 2,
                                SensorIsDeleted = false,
                            }
                        }
                    }
                };

                sensorSystemContext.Users.AddRange(Users);
                sensorSystemContext.Devices.AddRange(Devices);

                var UserDevices = new List<UserDevice>() {
                    new UserDevice() {
                        UserID = 1,
                        DeviceID = 1
                    },
                    new UserDevice() {
                        UserID = 1,
                        DeviceID = 2
                    },
                    new UserDevice() {
                        UserID = 1,
                        DeviceID = 3
                    },
                    new UserDevice() {
                        UserID = 2,
                        DeviceID = 1
                    },
                    new UserDevice() {
                        UserID = 3,
                        DeviceID = 1
                    },
                    new UserDevice() {
                        UserID = 3,
                        DeviceID = 2
                    }
                };

                sensorSystemContext.SaveChanges();

                sensorSystemContext.UserDevices.AddRange(UserDevices);

                sensorSystemContext.SaveChanges();
            }
        }
    }
}