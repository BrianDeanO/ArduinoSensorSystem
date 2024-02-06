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
    public class SensorController : Controller {

        private readonly ISensorRepository _sensorRepository;
        private readonly IMapper _mapper;
        private readonly IDeviceRepository _deviceRepository;
        private readonly ISensorDataRepository _sensorDataRepository;

        public SensorController(
            ISensorRepository sensorRepository, 
            IDeviceRepository deviceRepository, 
            IMapper mapper,
            ISensorDataRepository sensorDataRepository
        ) {
            _sensorRepository = sensorRepository;
            _mapper = mapper;
            _deviceRepository = deviceRepository;
            _sensorDataRepository = sensorDataRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Sensor>))]
        public IActionResult GetSensors() {
            var sensors = _mapper.Map<List<SensorDTO>>(_sensorRepository.GetSensors());

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(sensors);
            }
        }

        [HttpGet("{sensorId}")]
        [ProducesResponseType(200, Type = typeof(Sensor))]
        [ProducesResponseType(400)]
        public IActionResult GetSensor(int sensorId) {
            if(!_sensorRepository.SensorExists(sensorId)) {
                return NotFound();
            }

            var sensor = _mapper.Map<SensorDTO>(_sensorRepository.GetSensor(sensorId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(sensor);
            }
        }

        [HttpGet("{sensorId}/SensorDatas")]
        [ProducesResponseType(200, Type = typeof(ICollection<SensorData>))]
        [ProducesResponseType(400)]
        public IActionResult GetSensorDatas(int sensorId) {
            if(!_sensorRepository.SensorExists(sensorId)) {
                return NotFound();
            }

            var sensorDatas = _mapper.Map<List<SensorDataDTO>>(_sensorRepository.GetSensorDatas(sensorId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(sensorDatas);
            }
        }

        [HttpGet("{sensorId}/Device")]
        [ProducesResponseType(200, Type = typeof(Device))]
        [ProducesResponseType(400)]
        public IActionResult GetSensorDevice(int sensorId) {
            if(!_sensorRepository.SensorExists(sensorId)) {
                return NotFound();
            }

            var device = _mapper.Map<DeviceDTO>(_sensorRepository.GetSensorDevice(sensorId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(device);
            }
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateSensor([FromQuery] int deviceId, [FromBody] SensorDTO newSensor) {
            if(newSensor == null) {
                return BadRequest(ModelState);
            }

            var sensor = _sensorRepository.GetSensors()
                .Where(d => d.SensorName.Trim().ToUpper() == newSensor.SensorName.Trim().ToUpper())
                .FirstOrDefault();

            if(sensor != null) {
                ModelState.AddModelError("", "Sensor Already Exists.");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                var sensorMap = _mapper.Map<Sensor>(newSensor);

                sensorMap.DeviceID = deviceId;
                sensorMap.Device = _deviceRepository.GetDevice(deviceId);

                if(!_sensorRepository.CreateSensor(sensorMap)) {
                    ModelState.AddModelError("", "Something Went Wrong While Saving.");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully Created.");
            }
        }

        [HttpPut("{sensorId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateSensor(int sensorId, [FromBody] SensorDTO updatedSensor) {
            if(updatedSensor == null) {
                return BadRequest(ModelState);
            }

            else if(sensorId != updatedSensor.SensorID) {
                return BadRequest(ModelState);
            }

            else if(!_sensorRepository.SensorExists(sensorId)) {
                return NotFound();
            }

            else if(!ModelState.IsValid) {
                return BadRequest();
            }

            var sensorMap = _mapper.Map<Sensor>(updatedSensor);

            if(!_sensorRepository.UpdateSensor(sensorMap)) {
                ModelState.AddModelError("", "Something Went Wrong While Updating Sensor.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Updated.");
        }

        [HttpDelete("{sensorId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSensor(int sensorId) {
            if(!_sensorRepository.SensorExists(sensorId)) {
                return NotFound();
            }

            var sensorDatasToDelete = _sensorRepository.GetSensorDatas(sensorId);
            var sensorToDelete = _sensorRepository.GetSensor(sensorId);

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if(!_sensorDataRepository.DeleteSensorDatas(sensorDatasToDelete.ToList())) {
                ModelState.AddModelError("", "Something went wrong when deleting SensorDatas");
            }

            if(!_sensorRepository.DeleteSensor(sensorToDelete)) {
                ModelState.AddModelError("", "Something went wrong when deleting the Sensor");
            }

            return Ok("Successfully Deleted.");
        }
    }
}