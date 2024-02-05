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
using Newtonsoft.Json;
using System.Text.Json;

namespace backEndApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase {
        private readonly SensorSystemContext _context;

        public DevicesController(SensorSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceDTO>>> GetDevices() {
            return await _context.Devices
                .Select(x => DeviceToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{deviceId}")]
        public async Task<ActionResult<DeviceDTO>> GetDevice(int deviceId) {
            var device = await _context.Devices.FindAsync(deviceId);

            if (device == null) {
                return NotFound();
            }

            return DeviceToDTO(device);
        }

        [HttpPut("{deviceId}")]
        public async Task<IActionResult> PutDevice(int deviceId, Device deviceDTO) {
            if (deviceId != deviceDTO.DeviceID) {
                return BadRequest();
            }

            var device = await _context.Devices.FindAsync(deviceId);
            if (device == null) {
                return NotFound();
            }

            device.DeviceType = deviceDTO.DeviceType;
            device.DeviceName = deviceDTO.DeviceName;
            device.ZipCode = deviceDTO.ZipCode;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!DeviceExists(deviceId)) {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Device>> PostDevice(Device deviceDTO) {
            var device = new Device {
                DeviceIdent = deviceDTO.DeviceIdent,
                DeviceType = deviceDTO.DeviceType,
                DeviceName = deviceDTO.DeviceName,
                ZipCode = deviceDTO.ZipCode,
            };

            _context.Devices.Add(device);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetDevice),
                new { deviceId = device.DeviceID },
                DeviceToDTO(device));
        }

        [HttpDelete("{deviceId}")]
        public async Task<IActionResult> DeleteDevice(int deviceId) {
            var device = await _context.Devices.FindAsync(deviceId);

            // Not allowing a device deletion if it doesn't exist OR if it has an entry in the 
            //      deviceSensors table OR an entry in the userDevices table. (simulating Foreign key)
            if (device == null) {
                return NotFound();
            }
            else if(DeviceSensorExists(deviceId) || UserDeviceExists(deviceId)) {
                return Conflict();
            }

            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // This endpoint is only used by the device to check if a device by `deviceIdent`
        // already exists, so only return fields relevant to the device.
        [HttpGet("ident/{deviceIdent}")]
        public async Task<ActionResult<DeviceDTO>> GetDevice(String deviceIdent) {
            var device = await _context.Devices
                .Where(e => e.DeviceIdent == deviceIdent)
                .FirstOrDefaultAsync();

            if (device == null) {
                return NotFound();
            }

            var dto = new DeviceDTO {
                DeviceID = device.DeviceID
                // Poll time
            };
            return new JsonResult(dto, new JsonSerializerOptions() {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
        }

        private bool DeviceExists(int deviceId) {
            return _context.Devices.Any(e => e.DeviceID == deviceId);
        }

        private bool DeviceSensorExists(int deviceID) {
            return _context.DeviceSensors.Any(e => (e.DeviceID == deviceID));
        }
        private bool UserDeviceExists(int deviceID) {
            return _context.UserDevices.Any(e => (e.DeviceID == deviceID));
        }

        private static DeviceDTO DeviceToDTO(Device device) => new DeviceDTO {
            DeviceID = device.DeviceID,
            DeviceIdent = device.DeviceIdent,
            DeviceType = device.DeviceType,
            DeviceName = device.DeviceName,
            ZipCode = device.ZipCode,
        };
    }
}