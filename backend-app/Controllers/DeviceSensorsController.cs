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
    public class DeviceSensorsController : ControllerBase {
        private readonly SensorSystemContext _context;

        public DeviceSensorsController(SensorSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeviceSensorDTO>>> GetDeviceSensors() {
            return await _context.DeviceSensors
                .Select(x => DeviceSensorToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{deviceId}:{sensorId}")]
        public async Task<ActionResult<DeviceSensorDTO>> GetDeviceSensor(int deviceId, int sensorId) {
            var deviceSensor = await _context.DeviceSensors.FindAsync(deviceId, sensorId);

            if (deviceSensor == null) {
                return NotFound();
            } 

            else {
                return DeviceSensorToDTO(deviceSensor);
            }
        }
        
        [HttpPut("{deviceId}:{sensorId}")]
        public async Task<IActionResult> PutDeviceSensor(int deviceId, int sensorId, DeviceSensorDTO deviceSensorDTO) {
            if ((sensorId != deviceSensorDTO.SensorID) || deviceId != deviceSensorDTO.DeviceID) {
                return BadRequest();
            }

            var deviceSensor = await _context.DeviceSensors.FindAsync(deviceId, sensorId);
            if (deviceSensor == null) {
                return NotFound();
            }

            deviceSensor.SensorID = deviceSensorDTO.SensorID;
            deviceSensor.DeviceID = deviceSensorDTO.DeviceID;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!DeviceSensorExists(deviceId, sensorId)) {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<DeviceSensor>> PostDeviceSensor(DeviceSensor deviceSensorDTO) {
            var deviceSensor = new DeviceSensor {
                SensorID = deviceSensorDTO.SensorID,
                DeviceID = deviceSensorDTO.DeviceID,
            };

            // Checking to see if both the user and device exists (simulating foreign key)
            if(SensorExists(deviceSensorDTO.SensorID) && DeviceExists(deviceSensorDTO.DeviceID)) {
                _context.DeviceSensors.Add(deviceSensor);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetDeviceSensor),
                    new { sensorID = deviceSensor.SensorID, deviceID = deviceSensor.DeviceID },
                    DeviceSensorToDTO(deviceSensor));
            } else {
                return NotFound();
            }
        }

        [HttpDelete("{deviceId}:{sensorId}")]
        public async Task<IActionResult> DeleteDeviceSensor( int deviceId, int sensorId) {
            var deviceSensor = await _context.DeviceSensors.FindAsync(deviceId, sensorId);
            if (deviceSensor == null)
            {
                return NotFound();
            }

            _context.DeviceSensors.Remove(deviceSensor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeviceSensorExists(int deviceID, int sensorId) {
            return _context.DeviceSensors.Any(e => ((e.SensorID == sensorId) && (e.DeviceID == deviceID)));
        }

        private bool SensorExists(int id) {
            return _context.Sensors.Any(e => e.SensorID == id);
        }

        private bool DeviceExists(int id) {
            return _context.Devices.Any(e => e.DeviceID == id);
        }

        private static DeviceSensorDTO DeviceSensorToDTO(DeviceSensor deviceSensor) => new DeviceSensorDTO {
            SensorID = deviceSensor.SensorID,
            DeviceID = deviceSensor.DeviceID,
        };
    }
}