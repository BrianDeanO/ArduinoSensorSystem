using AutoMapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using backEndApp.DTO;
using backEndApp.Interfaces;
using backEndApp.Models;
using System.Text.Json;

namespace backEndApp.TestControllers {
    public class DeviceTestController : Controller {
        private readonly IDeviceRepository _deviceRepository;

        public DeviceTestController(
            IDeviceRepository deviceRepository

        ) {
            _deviceRepository = deviceRepository;
        }

        public ICollection<Device> GetDevices() {
            var devices = _deviceRepository.GetDevices();
            return devices;
        }

        public Device? GetDevice(int deviceID) {
            var device = _deviceRepository.GetDevice(deviceID);
            return device;  
        }

        public DeviceConfig GetDeviceConfigInfo(int deviceID) {
            var device = _deviceRepository.GetDevice(deviceID);

            if (device == null) {
                return new DeviceConfig() {
                    DeviceUpdateInterval = -1
                };
            }

            return new DeviceConfig() {
                DeviceUpdateInterval = device.DeviceUpdateInterval
            };
        }

        public Device CreateDevice(Device newDevice) {            
            var badDevice = new Device {
                DeviceID = -1,
                DeviceIdent = "BAD",
                DeviceName = "BAD",
                DeviceUpdateInterval = 0
            };

            if(!_deviceRepository.DeviceExists(newDevice.DeviceID)) {

                var defaultUpdateInterval = 60 * 60 * 24;
                newDevice.DeviceIsDeleted = false;
                newDevice.DeviceUpdateInterval = defaultUpdateInterval;

                var deviceCreated = _deviceRepository.CreateDevice(newDevice);
                if(deviceCreated) {
                    return newDevice;
                } else {
                    return badDevice;
                }
            } 

            return badDevice;
        }

        public Device UpdateDevice(Device device, string newDeviceName) {            
            var badDevice = new Device {
                DeviceID = -1,
                DeviceIdent = "BAD",
                DeviceName = "BAD",
                DeviceUpdateInterval = 0
            };

            if(!_deviceRepository.DeviceExists(device.DeviceID)) {
                device.DeviceName = newDeviceName;

                var deviceUpdated = _deviceRepository.UpdateDevice(device);
                if(deviceUpdated) {
                    return device;
                } else {
                    return badDevice;
                }
            } 

            return badDevice;
        }

        public bool DeleteDevice(Device device) {            
            if(!_deviceRepository.DeviceExists(device.DeviceID)) {
                var deviceDeleted = _deviceRepository.DeleteDevice(device);
                return deviceDeleted;
            } 

            return false;
        }
    }
}