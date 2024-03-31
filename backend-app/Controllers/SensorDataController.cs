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
using backEndApp.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace backEndApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SensorDataController : Controller {

        private readonly ISensorDataRepository _sensorDataRepository;
        private readonly IMapper _mapper;
        private readonly ISensorRepository _sensorRepository;
        private IHubContext<RealTimeDataHub> _hub;

        public SensorDataController(
            ISensorDataRepository sensorDataRepository, 
            ISensorRepository sensorRepository, 
            IMapper mapper,
            IHubContext<RealTimeDataHub> hub
        ) {
            _sensorDataRepository = sensorDataRepository;
            _mapper = mapper;
            _sensorRepository = sensorRepository;
            _hub = hub;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SensorData>))]
        public IActionResult GetSensorDatas() {
            var sensorDatas = _mapper.Map<List<SensorDataDTO>>(_sensorDataRepository.GetSensorDatas());

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(sensorDatas);
            }
        }

        [HttpGet("{sensorDataId}")]
        [ProducesResponseType(200, Type = typeof(SensorData))]
        [ProducesResponseType(400)]
        public IActionResult GetSensorData(int sensorDataId) {
            if(!_sensorDataRepository.SensorDataExists(sensorDataId)) {
                return NotFound();
            }

            var sensorData = _mapper.Map<SensorDataDTO>(_sensorDataRepository.GetSensorData(sensorDataId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(sensorData);
            }
        }

        [HttpGet("{sensorDataId}/Sensor")]
        [ProducesResponseType(200, Type = typeof(Sensor))]
        [ProducesResponseType(400)]
        public IActionResult GetSensorDataSensor(int sensorDataId) {
            if(!_sensorDataRepository.SensorDataExists(sensorDataId)) {
                return NotFound();
            }

            var sensorId = _sensorDataRepository.GetSensorData(sensorDataId).SensorID;
            var sensor = _mapper.Map<SensorDTO>(_sensorRepository.GetSensor(sensorId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(sensor);
            }
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

                var updateWebAppResult = _hub.Clients.All.SendAsync("frontEndWebApp", $"New Data Received @ {sensorDataMap.TimeRecorded}");

                return Ok("Successfully Created.");
            }
        }

        [HttpPut("{sensorDataId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateSensorData(int sensorDataId, [FromBody] SensorDataDTO updatedSensorData) {
            if(updatedSensorData == null) {
                return BadRequest(ModelState);
            }

            else if(sensorDataId != updatedSensorData.SensorDataID) {
                return BadRequest(ModelState);
            }

            else if(!_sensorDataRepository.SensorDataExists(sensorDataId)) {
                return NotFound();
            }

            else if(!ModelState.IsValid) {
                return BadRequest();
            }

            var sensorDataMap = _mapper.Map<SensorData>(updatedSensorData);

            if(!_sensorDataRepository.UpdateSensorData(sensorDataMap)) {
                ModelState.AddModelError("", "Something Went Wrong While Updating SensorData.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Updated.");
        }

        [HttpDelete("{sensorDataId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSensorData(int sensorDataId) {
            if(!_sensorDataRepository.SensorDataExists(sensorDataId)) {
                return NotFound();
            }

            var sensorDataToDelete = _sensorDataRepository.GetSensorData(sensorDataId);

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if(sensorDataToDelete == null) {
                ModelState.AddModelError("", "Something went wrong when getting the SensorData");
                return StatusCode(500, ModelState);
            }
            if(!_sensorDataRepository.DeleteSensorData(sensorDataToDelete)) {
                ModelState.AddModelError("", "Something went wrong when deleting the SensorData");
            }

            return Ok("Successfully Deleted.");
        }
    }
}