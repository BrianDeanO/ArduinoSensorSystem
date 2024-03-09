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
                    new User() {
                        UserType = "admin", 
                        UserFirstName = "Han",
                        UserLastName = "Solo",
                        UserPassword = "a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3",  // 123
                        UserEmail = "",
                        UserPhone = "",
                        UserNotifications = true
                    },
                    new User() {
                        UserType = "basic", 
                        UserFirstName = "Luke",
                        UserLastName = "Skywalker",
                        UserPassword = "b3a8e0e1f9ab1bfe3a36f231f676f78bb30a519d2b21e6c530c0eee8ebb4a5d0", // 456
                        UserEmail = "",
                        UserPhone = "",
                        UserNotifications = false
                    },
                    new User() {
                        UserType = "basic", 
                        UserFirstName = "Leia",
                        UserLastName = "Organa",
                        UserPassword = "35a9e381b1a27567549b5f8a6f783c167ebf809f1c4d6a9e367240484d8ce281", // 789
                        UserEmail = "",
                        UserPhone = "",
                        UserNotifications = true
                    },
                };
                var Devices = new List<Device>() {
                    new Device() {
                        DeviceIdent = "ARD-123",
                        DeviceName = "RICHLAND_NW_1",
                        DeviceType = "ARDUINO",
                        DeviceZipCode = "99352",
                        DeviceUpdateInterval = 86400000,

                        Sensors = new List<Sensor>() {
                            new Sensor() {
                                SensorIdent = "SEN-123",
                                SensorName = "BME_TRI_SENS", 
                                SensorType = "Adafruit BME280", 
                                ChannelCount = 3,
                                DeviceID = 1,
                                SensorDatas = new List<SensorData>() {
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 56, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-01T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 57, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-02T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 58, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-03T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 68, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-04T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 46, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-05T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 49, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-03-06T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 44,
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-02-01T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 52, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-02-02T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 51, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-02-03T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 57, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-02-04T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 62, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-02-05T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 61, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-02-07T18:42:27.069Z")
                                    },
                                    

                                    new SensorData() {
                                        ChannelID = 2,
                                        DataValue = 1300, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-03-01T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 1361, 
                                        DataUnit = "W/m^2",  
                                        TimeRecorded = DateTime.Parse("2024-03-02T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 1411, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-03-03T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 1391, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-03-04T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 1390, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-03-05T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 1200, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-02-06T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2,
                                        DataValue = 1310, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-02-01T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 1399, 
                                        DataUnit = "W/m^2",  
                                        TimeRecorded = DateTime.Parse("2024-02-02T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 1378, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-02-03T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 1290,
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-02-04T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 1368, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-02-05T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 1340, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-02-07T18:42:27.069Z")
                                    },


                                    new SensorData() {
                                        ChannelID = 3,
                                        DataValue = 30,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-03-01T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 3, 
                                        DataValue = 25,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-03-02T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 3, 
                                        DataValue = 24,
                                        DataUnit = "atm", 
                                        TimeRecorded = DateTime.Parse("2024-03-03T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 3, 
                                        DataValue = 31,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-03-04T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 3, 
                                        DataValue = 32,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-03-05T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 3, 
                                        DataValue = 29,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-03-06T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 3,
                                        DataValue = 30,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-02-01T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 3, 
                                        DataValue = 26,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-02-02T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 3, 
                                        DataValue = 28,
                                        DataUnit = "atm", 
                                        TimeRecorded = DateTime.Parse("2024-02-03T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 3, 
                                        DataValue = 27,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-02-04T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 3, 
                                        DataValue = 35,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-02-05T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 3, 
                                        DataValue = 23,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-02-07T18:42:27.069Z")
                                    },
                                }
                            },
                            new Sensor() {
                                SensorIdent = "SEN-456",
                                SensorName = "BME_2", 
                                SensorType = "Adafruit BME280", 
                                ChannelCount = 1,
                                DeviceID = 1,
                                SensorDatas = new List<SensorData>() {
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 1361, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-02-07T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 1459,
                                        DataUnit = "W/m^2",
                                        TimeRecorded = DateTime.Parse("2024-02-08T18:42:27.069Z")
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
                        
                        Sensors = new List<Sensor>() {
                            new Sensor() {
                                SensorIdent = "SEN-789",
                                SensorName = "BME_3", 
                                SensorType = "Adafruit BME280", 
                                ChannelCount = 2,
                                DeviceID = 2,
                                SensorDatas = new List<SensorData>() {
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 30,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-01-09T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 31,
                                        DataUnit = "atm", 
                                        TimeRecorded = DateTime.Parse("2024-01-10T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 33,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-01-11T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 29,
                                        DataUnit = "atm", 
                                        TimeRecorded = DateTime.Parse("2024-01-12T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
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
                                SensorDatas = new List<SensorData>() {
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 30,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-01-09T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 31,
                                        DataUnit = "atm", 
                                        TimeRecorded = DateTime.Parse("2024-01-10T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 33,
                                        DataUnit = "atm",
                                        TimeRecorded = DateTime.Parse("2024-01-11T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 29,
                                        DataUnit = "atm", 
                                        TimeRecorded = DateTime.Parse("2024-01-12T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
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

                        Sensors = new List<Sensor>() {
                            new Sensor() {
                                SensorIdent = "SEN-323",
                                SensorName = "BME_4", 
                                SensorType = "Adafruit BME280", 
                                ChannelCount = 2,
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