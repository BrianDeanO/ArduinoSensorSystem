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

namespace backEndApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : Controller {

        private readonly IDeviceRepository _deviceRepository;
        private readonly IMapper _mapper;

        public DeviceController(IDeviceRepository deviceRepository, IMapper mapper) {
            _deviceRepository = deviceRepository;
            _mapper = mapper;
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
                .Where(d => d.DeviceName.Trim().ToUpper() == newDevice.DeviceName.Trim().ToUpper())
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

                return Ok("Successfully Created.");
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

    }
}