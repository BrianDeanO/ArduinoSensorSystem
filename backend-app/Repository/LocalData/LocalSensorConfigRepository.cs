using backEndApp.DTO;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;

namespace backEndApp.Repository {
    public class LocalSensorConfigRepository: ISensorConfigRepository {
        private static List<SensorConfig> _configs = new() { };
        private static int _lastConfigId = 0;

        public ICollection<SensorConfig> GetSensorConfigs() {
            return _configs.OrderBy(s => s.SensorConfigID).ToList();
        }
        public ICollection<SensorConfig> GetSensorConfigs(int sensorId) {
            return _configs.Where(s => s.SensorID == sensorId).OrderBy(s => s.SensorConfigID).ToList();
        }
        public SensorConfig? GetSensorConfig(int sensorConfigID) {
            return _configs.Where(sc => sc.SensorConfigID == sensorConfigID).FirstOrDefault();
        }
        public bool SensorConfigExists(int sensorConfigID) {
            return _configs.Any(s => s.SensorConfigID == sensorConfigID);
        }
        
        public bool CreateSensorConfig(SensorConfig sensorConfig) {
            sensorConfig.SensorID = _lastConfigId++;
            _configs.Add(sensorConfig);
            return Save();
        }

        public bool UpdateSensorConfig(SensorConfig sensorConfig) {
            var old = _configs.Find(c => c.SensorConfigID == sensorConfig.SensorConfigID);
            if (old == null)
            {
                return false;
            }

            _configs.Remove(old);
            _configs.Add(sensorConfig);
            return Save();
        }

        public bool DeleteSensorConfig(SensorConfig sensorConfig) {
            _configs.Remove(sensorConfig);
            return Save();
        }

        public bool DeleteSensorConfigs(List<SensorConfig> sensorConfigs) {
            foreach (var sensor in sensorConfigs) {
                _configs.Remove(sensor);
            }
            return Save();
        }

        public bool Save() {
            return true;
        }
    }
}