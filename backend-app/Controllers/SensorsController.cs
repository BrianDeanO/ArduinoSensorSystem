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
using System.Text.Json;

namespace backEndApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SensorsController : ControllerBase {
        private readonly SensorSystemContext _context;

        public SensorsController(SensorSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorDTO>>> GetSensors() {
            return await _context.Sensors
                .Select(x => SensorToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{sensorId}")]
        public async Task<ActionResult<SensorDTO>> GetSensor(int sensorId) {
            var sensor = await _context.Sensors.FindAsync(sensorId);

            if (sensor == null) {
                return NotFound();
            }

            return SensorToDTO(sensor);
        }

        // This endpoint is only used by the device to check if a device by `deviceIdent`
        // already exists, so only return fields relevant to the device.
        [HttpGet("ident/{sensorIdent}")]
        public async Task<ActionResult<SensorDTO>> GetSensorIdent(String sensorIdent) {
            var sensor = await _context.Sensors
                .Where(e => e.SensorIdent == sensorIdent)
                .FirstOrDefaultAsync();

            if (sensor == null) {
                return NotFound();
            }

            var dto = new SensorDTO {
                SensorID = sensor.SensorID
                // Poll time
            };
            return new JsonResult(dto, new JsonSerializerOptions() {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
        }

        [HttpPut("{sensorId}")]
        public async Task<IActionResult> PutSensor(int sensorId, Sensor sensorDTO) {
            if (sensorId != sensorDTO.SensorID) {
                return BadRequest();
            }

            var sensor = await _context.Sensors.FindAsync(sensorId);
            if (sensor == null) {
                return NotFound();
            }

            sensor.SensorIdent = sensorDTO.SensorIdent;
            sensor.SensorType = sensorDTO.SensorType;
            sensor.SensorName = sensorDTO.SensorName;
            sensor.ChannelCount = sensorDTO.ChannelCount;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!SensorExists(sensorId)) {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Sensor>> PostSensor(Sensor sensorDTO) {
            var sensor = new Sensor {
                SensorIdent = sensorDTO.SensorIdent,
                SensorType = sensorDTO.SensorType,
                SensorName = sensorDTO.SensorName,
                ChannelCount = sensorDTO.ChannelCount,
            };

            _context.Sensors.Add(sensor);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetSensor),
                new { sensorId = sensor.SensorID },
                SensorToDTO(sensor));
        }

        [HttpDelete("{sensorId}")]
        public async Task<IActionResult> DeleteSensor(int sensorId) {
            var sensor = await _context.Sensors.FindAsync(sensorId);

            // Not allowing a sensor deletion if it doesn't exist OR if it has an entry in the 
            //      deviceSensors table OR an entry in the sensorDatas table. (simulating Foreign key)
            if (sensor == null) {
                return NotFound();
            }
            else if(DeviceSensorExists(sensorId) || SensorDataExists(sensorId)) {
                return Conflict();
            }

            _context.Sensors.Remove(sensor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SensorExists(int sensorId) {
            return _context.Sensors.Any(e => e.SensorID == sensorId);
        }

        private bool DeviceSensorExists(int sensorId) {
            return _context.DeviceSensors.Any(e => (e.SensorID == sensorId));
        }
        
        private bool SensorDataExists(int sensorId) {
            return _context.SensorDatas.Any(e => e.SensorID == sensorId);
        }

        private static SensorDTO SensorToDTO(Sensor sensor) => new SensorDTO {
            SensorID = sensor.SensorID,
            SensorIdent = sensor.SensorIdent,
            SensorType = sensor.SensorType,
            SensorName = sensor.SensorName,
            ChannelCount = sensor.ChannelCount,
        };
    }
}