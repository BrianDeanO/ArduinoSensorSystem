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
    public class UserDeviceController : Controller {

        private readonly IUserDeviceRepository _userDeviceRepository;
        private readonly IMapper _mapper;
        private readonly IDeviceRepository _deviceRepository;

        private readonly IUserRepository _userRepository;

        public UserDeviceController(
            IUserDeviceRepository userDeviceRepository, 
            IMapper mapper,
            IDeviceRepository deviceRepository,
            IUserRepository userRepository
        ) {
            _userDeviceRepository = userDeviceRepository;
            _mapper = mapper;
            _deviceRepository = deviceRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDevice>))]
        public IActionResult GetUserDevices() {
            var userDevices = _mapper.Map<List<UserDeviceDTO>>(_userDeviceRepository.GetUserDevices());

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(userDevices);
            }
        }

        [HttpGet("{userId}:{deviceId}")]
        [ProducesResponseType(200, Type = typeof(UserDevice))]
        [ProducesResponseType(400)]
        public IActionResult GetUserDevice(int userId, int deviceId) {
            if(!_userDeviceRepository.UserDeviceExists(userId, deviceId)) {
                return NotFound();
            }

            var userDevice = _mapper.Map<UserDeviceDTO>(_userDeviceRepository.GetUserDevice(userId, deviceId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(userDevice);
            }
        }

        [HttpGet("{userId}:{deviceId}/User")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUserDeviceUser(int userId, int deviceId) {
            if(!_userDeviceRepository.UserDeviceExists(userId, deviceId)) {
                return NotFound();
            }

            var user = _mapper.Map<UserDTO>(_userDeviceRepository.GetUserDeviceUser(userId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(user);
            }
        }

        [HttpGet("{userId}:{deviceId}/Device")]
        [ProducesResponseType(200, Type = typeof(Device))]
        [ProducesResponseType(400)]
        public IActionResult GetUserDeviceDevice(int userId, int deviceId) {
            if(!_userDeviceRepository.UserDeviceExists(userId, deviceId)) {
                return NotFound();
            }

            var device = _mapper.Map<DeviceDTO>(_userDeviceRepository.GetUserDeviceDevice(deviceId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(device); 
            }
        }

        [HttpGet("UserDevice/{userId}/Users")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        [ProducesResponseType(400)]
        public IActionResult GetUserDeviceUsers(int userId) {
            var users = _mapper.Map<List<UserDTO>>(_userDeviceRepository.GetUserDeviceUsers(userId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(users);
            }
        }

        [HttpGet("UserDevice/{deviceId}/Devices")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Device>))]
        [ProducesResponseType(400)]
        public IActionResult GetUserDeviceDevices(int deviceId) {
            // Make another UserDeviceExits????
            // if(!_userDeviceRepository.UserDeviceExists(userId, deviceId)) {
            //     return NotFound();
            // }

            var devices = _mapper.Map<List<DeviceDTO>>(_userDeviceRepository.GetUserDeviceDevices(deviceId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(devices);
            }             
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUserDevice(
            [FromQuery] int userId, 
            [FromQuery] int deviceId
        ) {
            var tempUserDevice = _userDeviceRepository.GetUserDevices()
                .Where(ud => ((ud.DeviceID == deviceId) && (ud.UserID == userId)))
                .FirstOrDefault();

            if(tempUserDevice != null) {
                ModelState.AddModelError("", "UserDevice Already Exists.");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {

                var userDevice = new UserDevice() {
                    UserID = userId,
                    User = _userRepository.GetUser(userId),
                    DeviceID = deviceId,
                    Device = _deviceRepository.GetDevice(deviceId),
                };

                if(!_userDeviceRepository.CreateUserDevice(userDevice)) {
                    ModelState.AddModelError("", "Something Went Wrong While Saving.");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully Created.");
            }
        }

        [HttpDelete("{userId}:{deviceId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUserDevice(int userId, int deviceId) {
            if(!_userRepository.UserDeviceExists(userId, deviceId)) {
                return NotFound();
            }

            var userDeviceToDelete = _userDeviceRepository.GetUserDevice(userId, deviceId);

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if(!_userDeviceRepository.DeleteUserDevice(userDeviceToDelete)) {
                ModelState.AddModelError("", "Something went wrong when deleting the UserDevice");
            }

            return Ok("Successfully Deleted.");
        }
        /*
            Doesn't make sense to have a update for this. 
                We would delete a UserDevice (i.e., take away access)
                Then add the UserDevice back (i.e., give them access)
        */
        // [HttpPut("{userId}:{deviceId}")]
        // [ProducesResponseType(204)]
        // [ProducesResponseType(400)]
        // [ProducesResponseType(404)]
        // public IActionResult UpdateUserDevice(int userId, int deviceId, [FromBody] UserDeviceDTO updatedUserDevice) {
        //     if(updatedUserDevice == null) {
        //         return BadRequest(ModelState);
        //     }

        //     else if(!_userDeviceRepository.UserDeviceExists(userId, deviceId)) {
        //         return NotFound();
        //     }

        //     else if((!_deviceRepository.DeviceExists(updatedUserDevice.DeviceID) || 
        //             !_userRepository.UserExists(updatedUserDevice.UserID)) ||
        //             (_userDeviceRepository.UserDeviceExists(updatedUserDevice.UserID, updatedUserDevice.DeviceID))
        //     ) {
        //         return BadRequest(ModelState);
        //     }

        //     else if(!ModelState.IsValid) {
        //         return BadRequest();
        //     }

        //     // var userDeviceMap = _mapper.Map<UserDevice>(updatedUserDevice);
        //     // userDeviceMap.User = _userRepository.GetUser(updatedUserDevice.UserID);
        //     // userDeviceMap.Device = _deviceRepository.GetDevice(updatedUserDevice.DeviceID);

        //     var userDevice = new UserDevice() {
        //         UserID = updatedUserDevice.UserID,
        //         User = _userRepository.GetUser(updatedUserDevice.UserID),
        //         DeviceID = updatedUserDevice.DeviceID,
        //         Device = _deviceRepository.GetDevice(updatedUserDevice.DeviceID),
        //     };

        //     if(!_userDeviceRepository.UpdateUserDevice(userDevice)) {
        //         ModelState.AddModelError("", "Something Went Wrong While Updating UserDevice.");
        //         return StatusCode(500, ModelState);
        //     }

        //     return Ok("Successfully Updated.");
        // }
    }
}