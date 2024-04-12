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

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUserDevice(
            [FromQuery] int userId, 
            [FromQuery] int deviceId
        ) {
            var tempUserDevice = _userDeviceRepository.GetUserDevices()
                .Where(ud => (ud.DeviceID == deviceId) && (ud.UserID == userId))
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
            if(!_userDeviceRepository.UserDeviceExists(userId, deviceId)) {
                return NotFound();
            }

            var userDeviceToDelete = _userDeviceRepository.GetUserDevice(userId, deviceId);

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if(userDeviceToDelete == null) {
                ModelState.AddModelError("", "Something went wrong when getting the UserDevice");
                return StatusCode(500, ModelState);
            }
            if(!_userDeviceRepository.DeleteUserDevice(userDeviceToDelete)) {
                ModelState.AddModelError("", "Something went wrong when deleting the UserDevice");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Deleted.");
        }
    }
}