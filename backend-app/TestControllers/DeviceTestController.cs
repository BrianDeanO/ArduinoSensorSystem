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
        private readonly IMapper _mapper;
        private readonly IUserDeviceRepository _userDeviceRepository;
        private readonly ISensorRepository _sensorRepository;
        private readonly IUserRepository _userRepository;

        public DeviceTestController(
            IDeviceRepository deviceRepository, 
            IMapper mapper,
            IUserDeviceRepository userDeviceRepository,
            ISensorRepository sensorRepository,
            IUserRepository userRepository
        ) {
            _deviceRepository = deviceRepository;
            _mapper = mapper;
            _userDeviceRepository = userDeviceRepository;
            _sensorRepository = sensorRepository;
            _userRepository = userRepository;
        }

        public DeviceTestController (
            IDeviceRepository deviceRepository
        ) {
            _deviceRepository = deviceRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Device>))]        
        public List<Device> GetDevices() {
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

        [HttpGet("{deviceId}")]
        [ProducesResponseType(200, Type = typeof(Device))]
        [ProducesResponseType(400)]
        public DeviceDTO GetDevice(int deviceId) {
            var device = new DeviceDTO{
                DeviceID = deviceId,
                DeviceIdent = "ARD-123",
                DeviceName = "PAWNEE_NW_1",
             };

            return device;  
        }
        
        [HttpGet("ident/{deviceIdent}")]
        public DeviceDTO GetDeviceIdent(string deviceIdent) {
            var device = new DeviceDTO{
                DeviceID = 1,
                DeviceIdent = deviceIdent,
                DeviceName = "PAWNEE_NW_1",
            };

            return device;
        }
        
        [HttpGet("{deviceId}/DeviceConfig")]
        [ProducesResponseType(200, Type = typeof(DeviceConfig))]
        [ProducesResponseType(400)]
        public IActionResult GetDeviceConfigInfo(int deviceId) {
            if(!_deviceRepository.DeviceExists(deviceId)) {
                return NotFound();
            }

            var device = _mapper.Map<DeviceDTO>(_deviceRepository.GetDevice(deviceId));

            var deviceConfig = new DeviceConfig {
                DeviceUpdateInterval = device.DeviceUpdateInterval
            };

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(deviceConfig);
            }
        }

        [HttpGet("{deviceId}/Sensors")]
        [ProducesResponseType(200, Type = typeof(ICollection<Sensor>))]
        [ProducesResponseType(400)]
        public IActionResult GetDeviceSensors(int deviceId) {
            if(!_deviceRepository.DeviceExists(deviceId)) {
                return NotFound();
            }

            var sensors = _deviceRepository.GetDeviceSensors(deviceId);

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(sensors);
            }
        }

        [HttpGet("{deviceId}/UserDevices")]
        [ProducesResponseType(200, Type = typeof(ICollection<UserDevice>))]
        [ProducesResponseType(400)]
        public IActionResult GetUserDevices(int deviceId) {
            if(!_deviceRepository.DeviceExists(deviceId)) {
                return NotFound();
            }

            var userDevices = _mapper.Map<List<UserDeviceDTO>>(_deviceRepository.GetUserDevices(deviceId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(userDevices);
            }
        }

        [HttpGet("{deviceId}/Users")]
        [ProducesResponseType(200, Type = typeof(ICollection<User>))]
        [ProducesResponseType(400)]
        public IActionResult GetUsersFromDevice(int deviceId) {
            if(!_deviceRepository.DeviceExists(deviceId)) {
                return NotFound();
            }

            var users = _mapper.Map<List<UserDTO>>(_deviceRepository.GetUsersFromDevice(deviceId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(users);
            }
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateDevice([FromBody] DeviceDTO newDevice) {            
            if(newDevice == null) {
                return BadRequest(ModelState);
            }

            var device = _deviceRepository.GetDevices()
                .Where(d => d.DeviceIdent == newDevice.DeviceIdent)
                .FirstOrDefault();

            if(device != null) {
                ModelState.AddModelError("", "Device Already Exists.");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                var defaultUpdateInterval = 60 * 60 * 24;
                
                var deviceMap = _mapper.Map<Device>(newDevice);
                deviceMap.DeviceIsDeleted = false;
                deviceMap.DeviceUpdateInterval = defaultUpdateInterval;

                if(!_deviceRepository.CreateDevice(deviceMap)) {
                    ModelState.AddModelError("", "Something Went Wrong While Saving.");
                    return StatusCode(500, ModelState);
                }

                var users = _userRepository.GetAdminUsers();

                foreach(var user in users) {
                    var userDevice = new UserDevice() {
                            UserID = user.UserID,
                            User = user,
                            DeviceID = deviceMap.DeviceID,
                            Device = deviceMap,
                        };

                    if(!_userDeviceRepository.CreateUserDevice(userDevice)) {
                        ModelState.AddModelError("", "Something Went Wrong While Saving the UserDevice.");
                        return StatusCode(500, ModelState);
                    }
                }

                var dto = new DeviceDTO {
                    DeviceID = deviceMap.DeviceID,
                    DeviceUpdateInterval = defaultUpdateInterval,
                };
                return new JsonResult(dto, new JsonSerializerOptions() {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
            }
        }

        [HttpPut("{deviceId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateDevice(int deviceId, [FromBody] DeviceDTO updatedDevice) {
            if(updatedDevice == null) {
                return BadRequest(ModelState);
            }

            else if(deviceId != updatedDevice.DeviceID) {
                ModelState.AddModelError("", "Include DeviceID..."); /// CUSTOM ERROR MESSAGE??????? POSSIBLY?
                return BadRequest(ModelState);
            }

            else if(!_deviceRepository.DeviceExists(deviceId)) {
                return NotFound();
            }

            else if(!ModelState.IsValid) {
                return BadRequest();
            }

            var deviceMap = _mapper.Map<Device>(updatedDevice);

            if(!_deviceRepository.UpdateDevice(deviceMap)) {
                ModelState.AddModelError("", "Something Went Wrong While Updating Device.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Updated.");
        }

        [HttpPost("Poke/{deviceId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult PokeDevice(int deviceId) {            
            if(!_deviceRepository.DeviceExists(deviceId)) {
                return NotFound();
            }

            var device = _deviceRepository.GetDevice(deviceId);
            if(device == null) {
                ModelState.AddModelError("", "No such device found");
                return StatusCode(422, ModelState);
            }

            device.DeviceLastSeen = DateTime.Now;

            if(!_deviceRepository.UpdateDevice(device)) {
                ModelState.AddModelError("", "Error while updating last seen time");
                return BadRequest(ModelState);
            } else {
                return Ok("Successfully Updated.");
            }
        }

        [HttpDelete("{deviceId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteDevice(int deviceId) {
            if(!_deviceRepository.DeviceExists(deviceId)) {
                return NotFound();
            }

            var userDevicesToDelete = _deviceRepository.GetUserDevices(deviceId);
            var deviceToDelete = _deviceRepository.GetDevice(deviceId);
            var sensorsToDelete = _deviceRepository.GetDeviceSensors(deviceId);

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if(!_userDeviceRepository.DeleteUserDevices(userDevicesToDelete.ToList())) {
                ModelState.AddModelError("", "Something went wrong when deleting UserDevices");
            }

            if(!_sensorRepository.DeleteSensors(sensorsToDelete.ToList())) {
                ModelState.AddModelError("", "Something went wrong when deleting Sensors");
            }

            if(!_deviceRepository.DeleteDevice(deviceToDelete)) {
                ModelState.AddModelError("", "Something went wrong when deleting the Device");
            }

            return Ok("Successfully Deleted.");
        }
    }
}