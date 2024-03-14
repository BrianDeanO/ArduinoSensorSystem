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
    public class SensorDataController : Controller {
        private readonly ISensorDataRepository _sensorDataRepository;
        private readonly IMapper _mapper;
        private readonly ISensorRepository _sensorRepository;

        public SensorDataController(ISensorDataRepository sensorDataRepository, ISensorRepository sensorRepository, IMapper mapper) {
            _sensorDataRepository = sensorDataRepository;
            _mapper = mapper;
            _sensorRepository = sensorRepository;
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateSensorData(
            [FromBody] SensorDataDTO newSensorData
        ) {
            if(newSensorData == null) {
                return BadRequest(ModelState);
            }

            var sensorData = _sensorDataRepository.GetSensorDatas()
                .Where(d =>
                    d.TimeRecorded == newSensorData.TimeRecorded
                    && d.SensorID == newSensorData.SensorID
                    && d.ChannelID == newSensorData.ChannelID
                )
                .FirstOrDefault();

            if(sensorData != null) {
                Console.WriteLine(newSensorData.TimeRecorded);
                ModelState.AddModelError("", "SensorData Already Exists.");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                var sensorDataMap = _mapper.Map<SensorData>(newSensorData);

                sensorDataMap.SensorID = newSensorData.SensorID;
                sensorDataMap.Sensor = _sensorRepository.GetSensor(newSensorData.SensorID);

                if(!_sensorDataRepository.CreateSensorData(sensorDataMap)) {
                    ModelState.AddModelError("", "Something Went Wrong While Saving.");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully Created.");
            }
        }
    }
}