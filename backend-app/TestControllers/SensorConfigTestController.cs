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
    public class SensorConfigTestController : Controller {
        private readonly ISensorConfigRepository _sensorConfigRepository;

        public SensorConfigTestController(
            ISensorConfigRepository sensorConfigRepository

        ) {
            _sensorConfigRepository = sensorConfigRepository;
        }

        public ICollection<SensorConfig> GetSensorConfigs() {
            var sensorConfigs = _sensorConfigRepository.GetSensorConfigs();
            return sensorConfigs;
        }

        public ICollection<SensorConfig> GetSensorConfigs(int sensorID) {
            var sensorConfigs = _sensorConfigRepository.GetSensorConfigs(sensorID);
            return sensorConfigs;
        }

        public SensorConfig? GetSensorConfig(int sensorConfigID) {
            var sensorConfig = _sensorConfigRepository.GetSensorConfig(sensorConfigID);
            return sensorConfig;  
        }

        public SensorConfig CreateSensorConfig(SensorConfig newSensorConfig) {            
            var badSensorConfig = new SensorConfig() {
                SensorConfigID = -1,
                SensorID = -1,
                SensorConfigKey = "BAD",
                SensorConfigValue = "BAD",
            };

            if(!_sensorConfigRepository.SensorConfigExists(newSensorConfig.SensorConfigID)) {

                var sensorConfigCreated = _sensorConfigRepository.CreateSensorConfig(newSensorConfig);
                if(sensorConfigCreated) {
                    return newSensorConfig;
                } else {
                    return badSensorConfig;
                }
            } 

            return badSensorConfig;
        }

        public SensorConfig UpdateSensorConfig(SensorConfig newSensorConfig, string newConfigValue) {            
            var badSensorConfig = new SensorConfig() {
                SensorConfigID = -1,
                SensorID = -1,
                SensorConfigKey = "BAD",
                SensorConfigValue = "BAD",
            };

            if(!_sensorConfigRepository.SensorConfigExists(newSensorConfig.SensorConfigID)) {
                newSensorConfig.SensorConfigValue = newConfigValue;

                var userUpdated = _sensorConfigRepository.UpdateSensorConfig(newSensorConfig);
                if(userUpdated) {
                    return newSensorConfig;
                } else {
                    return badSensorConfig;
                }
            } 

            return badSensorConfig;
        }

        public bool DeleteSensorConfig(SensorConfig sensorConfig) {            
            if(!_sensorConfigRepository.SensorConfigExists(sensorConfig.SensorConfigID)) {
                var sensorConfigDeleted = _sensorConfigRepository.DeleteSensorConfig(sensorConfig);
                return sensorConfigDeleted;
            } 

            return false;
        }
    }
}