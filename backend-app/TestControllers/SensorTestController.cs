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

namespace backEndApp.TestControllers {
    public class SensorTestController : Controller {
        private readonly ISensorRepository _sensorRepository;
        private readonly List<Device> _deviceDbContext;
        private readonly List<User> _userDbContext;
        private readonly List<Sensor> _sensorDbContext;
        private readonly List<SensorData> _sensorDataDbContext;
        private readonly List<SensorConfig> _sensorConfigDbContext;
        private readonly List<UserDevice> _userDeviceDbContext;

        public SensorTestController(
            ISensorRepository sensorRepository,
            // ISensorRepository sensorRepository,
            List<Device> deviceDbContext,
            List<User> userDbContext,
            List<Sensor> sensorDbContext,
            List<SensorData> sensorDataDbContext,
            List<SensorConfig> sensorConfigDbContext,
            List<UserDevice> userDeviceDbContext

        ) {
            _sensorRepository = sensorRepository;
            // _sensorRepository = sensorRepository;
            _deviceDbContext = deviceDbContext;
            _userDbContext = userDbContext;
            _sensorDbContext = sensorDbContext;
            _sensorDataDbContext = sensorDataDbContext;
            _sensorConfigDbContext = sensorConfigDbContext;
            _userDeviceDbContext = userDeviceDbContext;
        }

        public ICollection<Sensor> GetSensors() {
            var sensors = _sensorRepository.GetSensors();
            return sensors;
        }

        public Sensor? GetSensor(int sensorID) {
            var sensor = _sensorRepository.GetSensor(sensorID);
            return sensor;  
        }

        public ICollection<SensorConfig> GetSensorConfigs(int sensorID) {
            var sensorConfigs = _sensorRepository.GetSensorConfigs(sensorID);
            return sensorConfigs;
        }

        public ICollection<SensorData> GetSensorDatas(int sensorID) {
            var sensorDatas = _sensorRepository.GetSensorDatas(sensorID);
            return sensorDatas;
        }

        public Sensor CreateSensor(Sensor newSensor) {            
            var badSensor = new Sensor() {
                SensorID = -1,
                SensorIdent = "SEN-BAD",
                SensorName = "BAD"
            };

            if(!_sensorRepository.SensorExists(newSensor.SensorID)) {

                newSensor.SensorIsDeleted = false;

                var sensorCreated = _sensorRepository.CreateSensor(newSensor);
                if(sensorCreated) {
                    return newSensor;
                } else {
                    return badSensor;
                }
            } 

            return badSensor;
        }

        public Sensor UpdateSensor(Sensor newSensor, string newSensorName) {            
            var badSensor = new Sensor() {
                SensorID = -1,
                SensorIdent = "SEN-BAD",
                SensorName = "BAD"
            };

            if(!_sensorRepository.SensorExists(newSensor.SensorID)) {
                newSensor.SensorName = newSensorName;

                var sensorUpdated = _sensorRepository.UpdateSensor(newSensor);
                if(sensorUpdated) {
                    return newSensor;
                } else {
                    return badSensor;
                }
            } 

            return badSensor;
        }

        public bool DeleteSensor(Sensor sensor) {            
            if(!_sensorRepository.SensorExists(sensor.SensorID)) {
                var sensorDeleted = _sensorRepository.DeleteSensor(sensor);
                return sensorDeleted;
            } 

            return false;
        }
    }
}