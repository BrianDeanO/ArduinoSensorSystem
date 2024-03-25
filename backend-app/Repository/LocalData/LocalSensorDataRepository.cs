using backEndApp.DTO;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;
using System.Data;

namespace backEndApp.Repository {

    public class LocalSensorDataRepository: ISensorDataRepository {
        private static List<SensorData> _sensorData = new() {};

        public ICollection<SensorData> GetSensorDatas() {
            return _sensorData.OrderBy(sd => sd.SensorDataID).ToList();
        }

        public ICollection<SensorData> GetSensorDatas(int sensorId) {
            return _sensorData
                .Where(sd => sd.SensorID == sensorId)
                .OrderBy(sd => sd.SensorDataID)
                .ToList();
        }

        public bool CreateSensorData(SensorData sensorData) {
            if(_sensorData.Find(
                sd => 
                    sd.SensorDataID == sensorData.SensorDataID 
                    || (sd.TimeRecorded == sensorData.TimeRecorded 
                        && sd.ChannelID == sensorData.ChannelID)
            ) != null)
            {
                throw new DuplicateNameException("SensorData with id '" + sensorData.SensorDataID + "' already exists");
            }
            _sensorData.Add(sensorData);
            return Save();
        }
        
        public bool DeleteSensorDatas(List<SensorData> sensorDatas) {
            foreach (var sensorData in sensorDatas) {
                _sensorData.Remove(sensorData);
            }
            return Save();
        }

        public bool Save() {
            return true;
        }
    }
}