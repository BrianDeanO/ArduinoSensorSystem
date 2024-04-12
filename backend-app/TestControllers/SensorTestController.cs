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

        public SensorTestController(
            ISensorRepository sensorRepository

        ) {
            _sensorRepository = sensorRepository;
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