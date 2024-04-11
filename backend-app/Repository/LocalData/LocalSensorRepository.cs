using backEndApp.DTO;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;
using System.Data;

namespace backEndApp.Repository {

    public class LocalSensorRepository: ISensorRepository {
        private static List<Sensor> _sensors = new() {};
        private static List<SensorData> _sensorDatas = new() {};
        private static int _lastSensorId = 0;
        private static List<SensorConfig> _sensorConfigs = new() {};
        private static int _lastSensorConfigId = 0;
        
        public ICollection<Sensor> GetSensors() {
            return _sensors.OrderBy(s => s.SensorID).ToList();
        }

        public Sensor? GetSensor(int sensorId) {
            return _sensors.Where(s => s.SensorID == sensorId).FirstOrDefault();
        }

       public ICollection<SensorData> GetSensorDatas(int sensorId) {
            return _sensorDatas
                .Where(sd => sd.SensorID == sensorId)
                .OrderBy(sd => sd.SensorDataID)
                .ToList();
        }
        
        public ICollection<Sensor> GetDeviceSensors(int deviceId)
        {
            return _sensors
                .Where(s => s.DeviceID == deviceId)
                .OrderBy(s => s.SensorID)
                .ToList();
        }

        public ICollection<SensorConfig> GetSensorConfigs(int sensorId) {
            return _sensorConfigs.Where(sd => sd.SensorID == sensorId).ToList();
        }

        public bool CreateSensorConfig(SensorConfig sensorConfig) {
            sensorConfig.SensorConfigID = _lastSensorConfigId++;
            if (_sensorConfigs.Find(d => 
                d.SensorConfigID == sensorConfig.SensorConfigID && 
                d.SensorConfigKey == sensorConfig.SensorConfigKey
            ) != null) {
                throw new DuplicateNameException("Sensor with Config Key - '" + sensorConfig.SensorConfigKey + "' already exists");
            }
            _sensorConfigs.Add(sensorConfig);
            return Save();
        }

        public bool SensorExists(int sensorId) {
            return _sensors.Any(s => s.SensorID == sensorId);
        }
        
        public bool CreateSensor(Sensor sensor) {
            sensor.SensorID = _lastSensorId++;
            if (_sensors.Find(d => d.SensorIdent == sensor.SensorIdent) != null)
            {
                throw new DuplicateNameException("Sensor with ident '" + sensor.SensorIdent + "' already exists");
            }
            _sensors.Add(sensor);
            return Save();
        }

        public bool UpdateSensor(Sensor sensor) {
            var old = _sensors.Find(d => d.DeviceID == sensor.SensorID);
            if (old == null)
            {
                return false;
            }

            _sensors.Remove(old);
            _sensors.Add(sensor);
            return Save();
        }

        public bool DeleteSensor(Sensor sensor) {
            return _sensors.Remove(sensor);
        }

        public bool DeleteSensors(List<Sensor> sensors) {
            foreach (var sensor in sensors) {
                _sensors.Remove(sensor);
            }
            return Save();
        }

        public bool Save() {
            return true;
        }
    }
}