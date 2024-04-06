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
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace backEndApp.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : Controller {

        private readonly ISensorRepository _sensorRepository;
        private readonly IMapper _mapper;
        private readonly IDeviceRepository _deviceRepository;
        private readonly ISensorDataRepository _sensorDataRepository;
        private readonly ISensorConfigRepository _sensorConfigRepository;

        public SensorController(
            ISensorRepository sensorRepository, 
            IDeviceRepository deviceRepository, 
            IMapper mapper,
            ISensorDataRepository sensorDataRepository,
            ISensorConfigRepository sensorConfigRepository
        ) {
            _sensorRepository = sensorRepository;
            _mapper = mapper;
            _deviceRepository = deviceRepository;
            _sensorDataRepository = sensorDataRepository;
            _sensorConfigRepository = sensorConfigRepository;
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

        // This endpoint is only used by the device to check if a device by `deviceIdent`
        // already exists, so only return fields relevant to the device.
        [HttpGet("ident/{sensorIdent}")]
        public IActionResult GetSensorIdent(String sensorIdent) {
            var sensor = _sensorRepository.GetSensors()
                .Where(e => e.SensorIdent == sensorIdent)
                .FirstOrDefault();

            if (sensor == null) {
                return NotFound();
            }

            var dto = new SensorDTO {
                SensorID = sensor.SensorID
            };
            return new JsonResult(dto, new JsonSerializerOptions() {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            });
        }

        [HttpGet("{sensorId}/SensorConfigs")]
        [ProducesResponseType(200, Type = typeof(ICollection<SensorConfig>))]
        [ProducesResponseType(400)]
        public IActionResult GetSensorConfigs(int sensorId) {
            if(!_sensorRepository.SensorExists(sensorId)) {
                return NotFound();
            }

            var sensorConfigs = _mapper.Map<List<SensorConfigDTO>>(_sensorRepository.GetSensorConfigs(sensorId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(sensorConfigs);
            }
        }

        [HttpGet("{sensorId}/SensorDatas")]
        [ProducesResponseType(200, Type = typeof(ICollection<SensorData>))]
        [ProducesResponseType(400)]
        public IActionResult GetSensorDatas(int sensorId) {
            if(!_sensorRepository.SensorExists(sensorId)) {
                return NotFound();
            }

            var sensorDatas = _mapper.Map<List<SensorDataDTO>>(_sensorDataRepository.GetSensorDatas(sensorId));

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                return Ok(sensorDatas);
            }
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateSensor([FromBody] SensorWithConfigDTO newSensor) {
            if(newSensor == null) {
                return BadRequest(ModelState);
            }

            var sensor = _sensorRepository.GetSensors()
                .Where(d => d.SensorIdent == newSensor.SensorIdent)
                .FirstOrDefault();

            if(sensor != null) {
                ModelState.AddModelError("", "Sensor Already Exists.");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            } else {
                Sensor sensorMap = new Sensor {
                    DeviceID = newSensor.DeviceID,
                    Device = _deviceRepository.GetDevice(newSensor.DeviceID),
                    SensorIdent = newSensor.SensorIdent,
                    SensorName = newSensor.SensorName,
                    SensorType = newSensor.SensorType,
                    ChannelCount = newSensor.ChannelCount ?? 0,
                    SensorIsDeleted = false,
                    SensorConfigs = new List<SensorConfig>()
                };

                if(newSensor.SensorConfigs != null) {
                    foreach (var sensorConfig in newSensor.SensorConfigs) {
                        var sensorConfigMap = new SensorConfig() {
                            SensorID = sensorMap.SensorID,
                            SensorConfigKey = sensorConfig.Key,
                            SensorConfigValue = sensorConfig.Value,
                            Sensor = _sensorRepository.GetSensor(sensorMap.SensorID)
                        };

                        sensorMap.SensorConfigs.Add(sensorConfigMap);

                        if(!_sensorConfigRepository.CreateSensorConfig(sensorConfigMap)) {
                            ModelState.AddModelError("", "Something Went Wrong While Saving.");
                            return StatusCode(500, ModelState);
                        }
                    }
                }

                if(!_sensorRepository.CreateSensor(sensorMap)) {
                    ModelState.AddModelError("", "Something Went Wrong While Saving.");
                    return StatusCode(500, ModelState);
                }

                var dto = new SensorDTO {
                    SensorID = sensorMap.SensorID
                };
                return new JsonResult(dto, new JsonSerializerOptions() {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
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

            var sensorDatasToDelete = _sensorDataRepository.GetSensorDatas(sensorId);
            var sensorToDelete = _sensorRepository.GetSensor(sensorId);

            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            if(!_sensorDataRepository.DeleteSensorDatas(sensorDatasToDelete.ToList())) {
                ModelState.AddModelError("", "Something went wrong when deleting SensorDatas");
            }
            if(sensorToDelete == null) {
                ModelState.AddModelError("", "Something went wrong when getting the Sensor");
                return StatusCode(500, ModelState);
            }
            if(!_sensorRepository.DeleteSensor(sensorToDelete)) {
                ModelState.AddModelError("", "Something went wrong when deleting the Sensor");
            }

            return Ok("Successfully Deleted.");
        }
    }
}