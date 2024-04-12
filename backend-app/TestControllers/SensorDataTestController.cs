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
    public class SensorDataTestController : Controller {
        private readonly ISensorDataRepository _sensorDataRepository;

        public SensorDataTestController(
            ISensorDataRepository sensorConfigRepository

        ) {
            _sensorDataRepository = sensorConfigRepository;
        }

        public ICollection<SensorData> GetSensorDatas() {
            var sensorDatas = _sensorDataRepository.GetSensorDatas();
            return sensorDatas;
        }


        public SensorData? GetSensorData(int sensorConfigID) {
            var sensorConfig = _sensorDataRepository.GetSensorData(sensorConfigID);
            return sensorConfig;  
        }

        public SensorData CreateSensorData(SensorData newSensorData) {            
            var badSensorData = new SensorData() {
                SensorDataID = -1,
                SensorID = -1,
                ChannelID = -1, 
                DataValue = -1,
                DataUnit = "BAD", 
                TimeRecorded = DateTime.Parse("2077-08-09T18:42:27.069Z")
            };

            if(!_sensorDataRepository.SensorDataExists(newSensorData.SensorDataID)) {
                var sensorDataCreated = _sensorDataRepository.CreateSensorData(newSensorData);
                if(sensorDataCreated) {
                    return newSensorData;
                } else {
                    return badSensorData;
                }
            } 

            return badSensorData;
        }

        public SensorData UpdateSensorData(SensorData newSensorData, string newDataUnit) {            
            var badSensorData = new SensorData() {
                SensorDataID = -1,
                SensorID = -1,
                ChannelID = -1, 
                DataValue = -1,
                DataUnit = "BAD", 
                TimeRecorded = DateTime.Parse("2077-08-09T18:42:27.069Z")
            };

            if(!_sensorDataRepository.SensorDataExists(newSensorData.SensorDataID)) {
                newSensorData.DataUnit = newDataUnit;

                var dataUpdated = _sensorDataRepository.UpdateSensorData(newSensorData);
                if(dataUpdated) {
                    return newSensorData;
                } else {
                    return badSensorData;
                }
            } 

            return badSensorData;
        }

        public bool DeleteSensorData(SensorData sensorData) {            
            if(!_sensorDataRepository.SensorDataExists(sensorData.SensorDataID)) {
                var sensorDataDeleted = _sensorDataRepository.DeleteSensorData(sensorData);
                return sensorDataDeleted;
            } 

            return false;
        }
    }
}