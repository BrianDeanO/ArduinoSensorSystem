using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using backEndApp.Models;

namespace backEndApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly SensorSystemContext _context;

        public UsersController(SensorSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers() {
            return await _context.Users
                .Select(x => UserToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDTO>> GetUser(int userId) {
            var user = await _context.Users.FindAsync(userId);

            if (user == null) {
                return NotFound();
            }

            return UserToDTO(user);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> PutUser(int userId, User userDTO) {
            if (userId != userDTO.UserID) {
                return BadRequest();
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null) {
                return NotFound();
            }

            user.UserType = userDTO.UserType;
            user.UserName = userDTO.UserName;
            user.UserPassword = userDTO.UserPassword;
            user.UserEmail = userDTO.UserEmail;
            user.UserPhone = userDTO.UserPhone;
            user.UserNotifications = userDTO.UserNotifications;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UserExists(userId)) {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User userDTO) {
            var user = new User {
                UserType = userDTO.UserType,
                UserName = userDTO.UserName,
                UserPassword = userDTO.UserPassword,
                UserEmail = userDTO.UserEmail,
                UserPhone = userDTO.UserPhone,
                UserNotifications = userDTO.UserNotifications,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetUser),
                new { userId = user.UserID },
                UserToDTO(user));
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId) {
            var user = await _context.Users.FindAsync(userId);

            // Not allowing a user deletion if it doesn't exist OR if it has an entry in the 
            //      userDevices table (simulating Foreign key)
            if (user == null) {
                return NotFound();
            }
            else if(UserDeviceExists(userId)) {
                return Conflict();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int userId) {
            return _context.Users.Any(e => e.UserID == userId);
        }
        private bool UserDeviceExists(int userId) {
            return _context.UserDevices.Any(e => (e.UserID == userId));
        }

        private static UserDTO UserToDTO(User user) => new UserDTO {
            UserID = user.UserID,
            UserType = user.UserType,
            UserName = user.UserName,
            UserPassword = user.UserPassword,
            UserEmail = user.UserEmail,
            UserPhone = user.UserPhone,
            UserNotifications = user.UserNotifications,
        };
    }
}