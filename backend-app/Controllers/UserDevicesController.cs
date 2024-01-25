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
    public class UserDevicesController : ControllerBase {
        private readonly SensorSystemContext _context;

        public UserDevicesController(SensorSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDeviceDTO>>> GetUserDevices() {
            return await _context.UserDevices
                .Select(x => UserDeviceToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{userId}:{deviceId}")]
        public async Task<ActionResult<UserDeviceDTO>> GetUserDevice(int userId, int deviceId) {
            var userDevice = await _context.UserDevices.FindAsync(userId, deviceId);

            if (userDevice == null) {
                return NotFound();
            } 

            return UserDeviceToDTO(userDevice);
        }

        [HttpPut("{userId}:{deviceId}")]
        public async Task<IActionResult> PutUserDevice(int userId, int deviceId, UserDeviceDTO userDeviceDTO) {
            if ((userId != userDeviceDTO.UserID) || deviceId != userDeviceDTO.DeviceID) {
                return BadRequest();
            }

            var userDevice = await _context.UserDevices.FindAsync(userId, deviceId);
            if (userDevice == null) {
                return NotFound();
            }

            userDevice.UserID = userDeviceDTO.UserID;
            userDevice.DeviceID = userDeviceDTO.DeviceID;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UserDeviceExists(userId, deviceId)) {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<UserDevice>> PostUserDevice(UserDevice userDeviceDTO) {
            var userDevice = new UserDevice {
                UserID = userDeviceDTO.UserID,
                DeviceID = userDeviceDTO.DeviceID,
            };

            // Checking to see if both the user and device exists (simulating foreign key)
            if(UserExists(userDeviceDTO.UserID) && DeviceExists(userDeviceDTO.DeviceID)) {
                _context.UserDevices.Add(userDevice);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetUserDevice),
                    new { userID = userDevice.UserID, deviceID = userDevice.DeviceID },
                    UserDeviceToDTO(userDevice));
            } else {
                return NotFound();
            }

        }

        [HttpDelete("{userId}:{deviceId}")]
        public async Task<IActionResult> DeleteUserDevice(int userId, int deviceId) {
            var userDevice = await _context.UserDevices.FindAsync(userId, deviceId);
            if (userDevice == null)
            {
                return NotFound();
            }

            _context.UserDevices.Remove(userDevice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserDeviceExists(int userId, int deviceID) {
            return _context.UserDevices.Any(e => ((e.UserID == userId) && (e.DeviceID == deviceID)));
        }

        private bool UserExists(int id) {
            return _context.Users.Any(e => (e.UserID == id));
        }

        private bool DeviceExists(int id) {
            return _context.Devices.Any(e => (e.DeviceID == id));
        }

        private static UserDeviceDTO UserDeviceToDTO(UserDevice userDevice) => new UserDeviceDTO {
            UserID = userDevice.UserID,
            DeviceID = userDevice.DeviceID,
        };
    }
}