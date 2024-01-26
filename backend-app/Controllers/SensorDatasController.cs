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
    public class SensorDatasController : ControllerBase {
        private readonly SensorSystemContext _context;

        public SensorDatasController(SensorSystemContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorDataDTO>>> GetSensorDatas() {
            return await _context.SensorDatas
                .Select(x => SensorDataToDTO(x))
                .ToListAsync();
        }

        [HttpGet("{sensorDataID}")]
        public async Task<ActionResult<SensorDataDTO>> GetSensorData(int sensorDataID) {
            var sensorData = await _context.SensorDatas.FindAsync(sensorDataID);

            if (sensorData == null) {
                return NotFound();
            }

            return SensorDataToDTO(sensorData);
        }

        // [HttpGet("{sensorId}")]
        // public async Task<ActionResult<SensorDTO>> GetSensor(int sensorId) {
        //     var sensor = await _context.Sensors.FindAsync(sensorId);

        //     if (sensor == null) {
        //         return NotFound();
        //     }

        //     return SensorToDTO(sensor);
        // }

        [HttpPut("{sensorDataID}")]
        public async Task<IActionResult> PutSensorData(int sensorDataID, SensorData sensorDataDTO) {
            if (sensorDataID != sensorDataDTO.SensorDataID) {
                return BadRequest();
            }

            var sensorData = await _context.SensorDatas.FindAsync(sensorDataID);
            if (sensorData == null) {
                return NotFound();
            }

            sensorData.ChannelID = sensorDataDTO.ChannelID;
            sensorData.DataValue = sensorDataDTO.DataValue;
            sensorData.DataUnit = sensorDataDTO.DataUnit;
            sensorData.TimeRecorded = sensorDataDTO.TimeRecorded;
            sensorData.SensorID = sensorDataDTO.SensorID;

            try {
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException) when (!SensorDataExists(sensorDataID)) {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<SensorData>> PostSensorData(SensorData sensorDataDTO) {

            // Throwing exception if the ID of the sensor does not correspond to a sensor entry in the sensor table
            //  i.e., simulating a foreign key check
            if(DoesSensorDataSensorExists(sensorDataDTO.SensorID)) {
                var sensorData = new SensorData {
                    ChannelID = sensorDataDTO.ChannelID,
                    DataValue = sensorDataDTO.DataValue,
                    DataUnit = sensorDataDTO.DataUnit,
                    TimeRecorded = sensorDataDTO.TimeRecorded,
                    SensorID = sensorDataDTO.SensorID,
                };

                _context.SensorDatas.Add(sensorData);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetSensorData),
                    new { sensorDataID = sensorData.SensorDataID },
                    SensorDataToDTO(sensorData));
            } else {
                return NotFound();
            }
        }

        [HttpDelete("{sensorDataID}")]
        public async Task<IActionResult> DeleteSensorData(int sensorDataID) {
            var sensorData = await _context.SensorDatas.FindAsync(sensorDataID);
            if (sensorData == null)
            {
                return NotFound();
            }

            _context.SensorDatas.Remove(sensorData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SensorDataExists(int sensorDataID) {
            return _context.SensorDatas.Any(e => e.SensorDataID == sensorDataID);
        }

        private bool DoesSensorDataSensorExists(int sensorID) {
            return _context.Sensors.Any(e => e.SensorID == sensorID);
        }

        private static SensorDataDTO SensorDataToDTO(SensorData sensorData) => new SensorDataDTO {
            SensorDataID = sensorData.SensorDataID,
            ChannelID = sensorData.ChannelID,
            DataValue = sensorData.DataValue,
            DataUnit = sensorData.DataUnit,
            TimeRecorded = sensorData.TimeRecorded,
            SensorID = sensorData.SensorID,
        };
        
        // private static SensorDTO SensorToDTO(Sensor sensor) => new SensorDTO {
        //     SensorID = sensor.SensorID,
        //     SensorType = sensor.SensorType,
        //     SensorName = sensor.SensorName,
        //     ChannelCount = sensor.ChannelCount,
        // };
    }
}