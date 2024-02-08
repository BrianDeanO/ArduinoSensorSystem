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
                        UserPassword = "123", 
                        UserEmail = "",
                        UserPhone = "",
                        UserNotifications = true
                    },
                    new User() {
                        UserType = "basic", 
                        UserFirstName = "Luke",
                        UserLastName = "Skywalker",
                        UserPassword = "456", 
                        UserEmail = "",
                        UserPhone = "",
                        UserNotifications = false
                    },
                    new User() {
                        UserType = "basic", 
                        UserFirstName = "Leia",
                        UserLastName = "Organa",
                        UserPassword = "789", 
                        UserEmail = "",
                        UserPhone = "",
                        UserNotifications = true
                    },
                };
                var Devices = new List<Device>() {
                    new Device() {
                        DeviceName = "RICHLAND_NW_1",
                        DeviceType = "ARDUINO",
                        ZipCode = "99352",
                        Sensors = new List<Sensor>() {
                            new Sensor() {
                                SensorName = "TEMP_1", 
                                SensorType = "Temperature", 
                                ChannelCount = 2,
                                DeviceID = 1,
                                SensorDatas = new List<SensorData>() {
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 13, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-01-01T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 14, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-01-02T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 15, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-01-03T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 12, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-01-04T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 2, 
                                        DataValue = 16, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-01-05T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 19, 
                                        DataUnit = "F", 
                                        TimeRecorded = DateTime.Parse("2024-01-06T18:42:27.069Z")
                                    },
                                }
                            },
                            new Sensor() {
                                SensorName = "IRRAD_1", 
                                SensorType = "Irradiance", 
                                ChannelCount = 1,
                                DeviceID = 1,
                                SensorDatas = new List<SensorData>() {
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 1361, 
                                        DataUnit = "W/m^2", 
                                        TimeRecorded = DateTime.Parse("2024-01-07T18:42:27.069Z")
                                    },
                                    new SensorData() {
                                        ChannelID = 1, 
                                        DataValue = 1459,
                                        DataUnit = "W/m^2",
                                        TimeRecorded = DateTime.Parse("2024-01-08T18:42:27.069Z")
                                    },
                                }
                            }
                        }
                    },
                    new Device() {
                        DeviceName = "KENNEWICK_N_1",
                        DeviceType = "ARDUINO",
                        ZipCode = "99336",
                        
                        Sensors = new List<Sensor>() {
                            new Sensor() {
                                SensorName = "PRESSURE_1", 
                                SensorType = "Pressure", 
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
                                SensorName = "PRESSURE_1", 
                                SensorType = "Pressure", 
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
                        DeviceName = "PASCO_E_1",
                        DeviceType = "ARDUINO",
                        ZipCode = "99301",
                        Sensors = new List<Sensor>() {
                            new Sensor() {
                                SensorName = "TEMP_2", 
                                SensorType = "Temperature", 
                                ChannelCount = 2,
                            }
                        }
                    }
                };

                sensorSystemContext.Users.AddRange(Users);
                sensorSystemContext.Devices.AddRange(Devices);

                sensorSystemContext.SaveChanges();
            }
        }
    }
}