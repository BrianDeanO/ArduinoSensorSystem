using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using backEndApp.DTO;
using backEndApp.Interfaces;
using backEndApp.Models;
using System.Text.Json;

namespace backEndApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SensorConfigController: Controller {
        private readonly ISensorConfigRepository _sensorConfigRepository;
        private readonly ISensorRepository _sensorRepository;
        private readonly IMapper _mapper;

        public SensorConfigController(ISensorConfigRepository sensorConfigRepository, ISensorRepository sensorRepository, IMapper mapper) {
            _sensorConfigRepository = sensorConfigRepository;
            _sensorRepository = sensorRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<SensorConfig>))]
        public IActionResult GetSensorConfigs() {
            var sensorConfigs = _mapper.Map<List<SensorConfigDTO>>(_sensorConfigRepository.GetSensorConfigs());

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(sensorConfigs);
            }
        }

        [HttpGet("ForSensor/{sensorId}")]
        [ProducesResponseType(200, Type = typeof(Dictionary<String, String>))]
        public IActionResult GetSensorConfigs(int sensorId) {
            var sensorConfigs = _sensorConfigRepository.GetSensorConfigs(sensorId);
            var dict = new Dictionary<String, String>();
            foreach (var sensorConfig in sensorConfigs) {
                dict.Add(sensorConfig.SensorConfigKey, sensorConfig.SensorConfigValue);
            }

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(dict);
            }
        }

        [HttpGet("{sensorConfigId}")]
        [ProducesResponseType(200, Type = typeof(SensorConfig))]
        [ProducesResponseType(400)]
        public IActionResult GetSensorConfig(int sensorConfigId) {
            if(!_sensorConfigRepository.SensorConfigExists(sensorConfigId)) {
                return NotFound();
            }

            var sensorConfig = _mapper.Map<SensorConfigDTO>(_sensorConfigRepository.GetSensorConfig(sensorConfigId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(sensorConfig);
            }
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateSensorConfig(
            [FromBody] SensorConfigDTO newSensorConfig
        ) {
            if(newSensorConfig == null) {
                return BadRequest(ModelState);
            }

            var sensorConfig = _sensorConfigRepository.GetSensorConfigs()
                .Where(sc =>
                    sc.SensorID == newSensorConfig.SensorID
                    && sc.SensorConfigKey == newSensorConfig.SensorConfigKey
                )
                .FirstOrDefault();

            if(sensorConfig != null) {
                ModelState.AddModelError("", "SensorConfig Already Exists.");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                var sensorConfigMap = _mapper.Map<SensorConfig>(newSensorConfig);

                sensorConfigMap.SensorID = newSensorConfig.SensorID;
                sensorConfigMap.Sensor = _sensorRepository.GetSensor(newSensorConfig.SensorID);

                if(!_sensorConfigRepository.CreateSensorConfig(sensorConfigMap)) {
                    ModelState.AddModelError("", "Something Went Wrong While Saving.");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully Created.");
            }
        }

        [HttpPut("{sensorConfigId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateSensorConfig(int sensorConfigId, [FromBody] SensorConfigDTO updatedSensorConfig) {
            if(updatedSensorConfig == null) {
                return BadRequest(ModelState);
            }

            else if(sensorConfigId != updatedSensorConfig.SensorConfigID) {
                return BadRequest(ModelState);
            }

            else if(!_sensorConfigRepository.SensorConfigExists(sensorConfigId)) {
                return NotFound();
            }

            else if(!ModelState.IsValid) {
                return BadRequest();
            }

            var sensorConfigMap = _mapper.Map<SensorConfig>(updatedSensorConfig);

            if(!_sensorConfigRepository.UpdateSensorConfig(sensorConfigMap)) {
                ModelState.AddModelError("", "Something Went Wrong While Updating SensorConfig.");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully Updated.");
        }

        [HttpDelete("{sensorConfigId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSensorConfig(int sensorConfigId) {
            if(!_sensorConfigRepository.SensorConfigExists(sensorConfigId)) {
                return NotFound();
            }

            var sensorConfigToDelete = _sensorConfigRepository.GetSensorConfig(sensorConfigId);

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if(sensorConfigToDelete == null) {
                ModelState.AddModelError("", "Something went wrong when getting the SensorConfig");
                return StatusCode(500, ModelState);
            }
            if(!_sensorConfigRepository.DeleteSensorConfig(sensorConfigToDelete)) {
                ModelState.AddModelError("", "Something went wrong when deleting the SensorConfig");
            }

            return Ok("Successfully Deleted.");
        }
    }

}