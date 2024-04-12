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
    public class UserController : Controller {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IUserDeviceRepository _userDeviceRepository;

        public UserController(
            IUserRepository userRepository, 
            IMapper mapper,
            IDeviceRepository deviceRepository,
            IUserDeviceRepository userDeviceRepository
        ) {
            _userRepository = userRepository;
            _mapper = mapper;
            _deviceRepository = deviceRepository;
            _userDeviceRepository = userDeviceRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUsers() {
            var users = _mapper.Map<List<UserDTO>>(_userRepository.GetUsers());

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(users);
            }
        }
        
        [HttpGet("ADMIN")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetAdminUsers() {
            var users = _mapper.Map<List<UserDTO>>(_userRepository.GetAdminUsers());

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(users);
            }
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(int userId) {
            if(!_userRepository.UserExists(userId)) {
                return NotFound();
            }

            var user = _mapper.Map<UserDTO>(_userRepository.GetUser(userId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(user);
            }
        }

        [HttpGet("{userFirstName}.{userLastName}:{userPassword}")]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public IActionResult GetUserWithLogin(
            string userFirstName, 
            string userLastName,
            string userPassword
        ) {
            var user = _mapper.Map<UserDTO>(_userRepository.GetUserWithLogin(userFirstName, userLastName, userPassword));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(user);
            }
        }

        [HttpGet("{userId}/Devices")]
        [ProducesResponseType(200, Type = typeof(ICollection<Device>))]
        [ProducesResponseType(400)]
        public IActionResult GetDevicesFromUser(int userId) {
            if(!_userRepository.UserExists(userId)) {
                return NotFound();
            }

            var devices = _mapper.Map<List<DeviceDTO>>(_deviceRepository.GetDevicesForUser(userId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(devices); 
            }
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateUser([FromBody] UserDTO newUser) {
            if(newUser == null) {
                return BadRequest(ModelState);
            }

            else if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } 

            else {
                var tempUser = _userRepository.GetUserWithLogin(newUser.UserFirstName, newUser.UserLastName, newUser.UserPassword);
                
                if(tempUser != null) {
                    ModelState.AddModelError("", "New User Info Matches Another User.");
                    return BadRequest(ModelState);
                }

                var userMap = _mapper.Map<User>(newUser);
                userMap.UserIsDeleted = false;

                if(!_userRepository.CreateUser(userMap)) {
                    ModelState.AddModelError("", "Something Went Wrong While Saving.");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully Created.");
            }
        }

        [HttpPut("{userId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateUser(int userId, [FromBody] UserDTO updatedUser) {
            if(updatedUser == null) {
                return BadRequest(ModelState);
            }

            else if(userId != updatedUser.UserID) {
                return BadRequest(ModelState);
            }

            else if(!_userRepository.UserExists(userId)) {
                return NotFound();
            }

            else if(!ModelState.IsValid) {
                return BadRequest();
            }

            else {
                var tempUser = _userRepository.GetUserWithLogin(updatedUser.UserFirstName, updatedUser.UserLastName, updatedUser.UserPassword);
                
                if(tempUser != null) {
                    ModelState.AddModelError("", "New User Info Matches Another User.");
                    return BadRequest(ModelState);
                }

                var userMap = _mapper.Map<User>(updatedUser);

                if(!_userRepository.UpdateUser(userMap)) {
                    ModelState.AddModelError("", "Something Went Wrong While Updating User.");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully Updated.");
            }


        }

        [HttpDelete("{userId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int userId) {
            if(!_userRepository.UserExists(userId)) {
                return NotFound();
            }

            var userDevicesToDelete = _userDeviceRepository.GetUserDevices(userId);
            var userToDelete = _userRepository.GetUser(userId);

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if(!_userDeviceRepository.DeleteUserDevices(userDevicesToDelete.ToList())) {
                ModelState.AddModelError("", "Something went wrong when deleting UserDevices");
            }
            if(userToDelete == null) {
                ModelState.AddModelError("", "Something went wrong when getting the User");
                return StatusCode(500, ModelState);
            }
            if(!_userRepository.DeleteUser(userToDelete)) {
                ModelState.AddModelError("", "Something went wrong when deleting the User");
            }

            return Ok("Successfully Deleted.");
        }
    }
}