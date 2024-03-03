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

namespace backEndApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : Controller {

        private readonly IDeviceRepository _deviceRepository;
        private readonly IMapper _mapper;
        private readonly IUserDeviceRepository _userDeviceRepository;
        private readonly ISensorRepository _sensorRepository;

        public DeviceController(
            IDeviceRepository deviceRepository, 
            IMapper mapper,
            IUserDeviceRepository userDeviceRepository,
            ISensorRepository sensorRepository
        ) {
            _deviceRepository = deviceRepository;
            _mapper = mapper;
            _userDeviceRepository = userDeviceRepository;
            _sensorRepository = sensorRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Device>))]
        public IActionResult GetDevices() {
            var devices = _mapper.Map<List<DeviceDTO>>(_deviceRepository.GetDevices());

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(devices);
            }
        }

        [HttpGet("{deviceId}")]
        [ProducesResponseType(200, Type = typeof(Device))]
        [ProducesResponseType(400)]
        public IActionResult GetDevice(int deviceId) {
            if(!_deviceRepository.DeviceExists(deviceId)) {
                return NotFound();
            }

            var device = _mapper.Map<DeviceDTO>(_deviceRepository.GetDevice(deviceId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(device);
            }
        }

        // This endpoint is only used by the device to check if a device by `deviceIdent`
        // already exists, so only return fields relevant to the device.
        [HttpGet("ident/{deviceIdent}")]
        public IActionResult GetDevice(String deviceIdent) {
            Console.WriteLine("DeviceIdent: " + deviceIdent);
            var device = _deviceRepository.GetDevices() 
                .Where(e => e.DeviceIdent == deviceIdent)
                .FirstOrDefault();

            if (device == null) {
                return NotFound();
            }

            var dto = new DeviceDTO {
                DeviceID = device.DeviceID
            };
            return new JsonResult(dto, new JsonSerializerOptions() {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
        }

        [HttpGet("DeviceName")]
        [ProducesResponseType(200, Type = typeof(Device))]
        [ProducesResponseType(400)]
        public IActionResult GetDeviceWithName([FromQuery] string deviceName) {
            var device = _mapper.Map<DeviceDTO>(_deviceRepository.GetDeviceWithName(deviceName));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(device);
            }
        }

        
        [HttpGet("DeviceConfigInfo")]
        [ProducesResponseType(200, Type = typeof(DeviceConfig))]
        [ProducesResponseType(400)]
        public IActionResult GetDeviceConfigInfo(int deviceId) {
            if(!_deviceRepository.DeviceExists(deviceId)) {
                return NotFound();
            }

            var device = _mapper.Map<DeviceDTO>(_deviceRepository.GetDevice(deviceId));

            var deviceConfig = new DeviceConfig {
                DevicePollingInterval = device.DevicePollingInterval
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
                
                var deviceMap = _mapper.Map<Device>(newDevice);

                if(!_deviceRepository.CreateDevice(deviceMap)) {
                    ModelState.AddModelError("", "Something Went Wrong While Saving.");
                    return StatusCode(500, ModelState);
                }

                var defaultPollingInterval = 1000 * 60 * 60 * 24;
                
                var dto = new DeviceDTO {
                    DeviceID = deviceMap.DeviceID,
                    DevicePollingInterval = defaultPollingInterval.ToString(),
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