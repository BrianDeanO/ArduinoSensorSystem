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
        // private readonly ISensorRepository _sensorRepository;
        private readonly List<Device> _deviceDbContext;
        private readonly List<User> _userDbContext;
        private readonly List<Sensor> _sensorDbContext;
        private readonly List<SensorData> _sensorDataDbContext;
        private readonly List<SensorConfig> _sensorConfigDbContext;
        private readonly List<UserDevice> _userDeviceDbContext;

        public SensorConfigTestController(
            ISensorConfigRepository sensorConfigRepository,
            // ISensorRepository sensorRepository,
            List<Device> deviceDbContext,
            List<User> userDbContext,
            List<Sensor> sensorDbContext,
            List<SensorData> sensorDataDbContext,
            List<SensorConfig> sensorConfigDbContext,
            List<UserDevice> userDeviceDbContext

        ) {
            _sensorConfigRepository = sensorConfigRepository;
            // _sensorRepository = sensorRepository;
            _deviceDbContext = deviceDbContext;
            _userDbContext = userDbContext;
            _sensorDbContext = sensorDbContext;
            _sensorDataDbContext = sensorDataDbContext;
            _sensorConfigDbContext = sensorConfigDbContext;
            _userDeviceDbContext = userDeviceDbContext;
        }

        public ICollection<SensorConfig> GetSensorConfigs() {
            var sensorConfigs = _sensorConfigRepository.GetSensorConfigs();
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