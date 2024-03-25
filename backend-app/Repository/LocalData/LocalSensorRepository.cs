using backEndApp.DTO;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;
using System.Data;

namespace backEndApp.Repository {

    public class LocalSensorRepository: ISensorRepository {
        private static List<Sensor> _sensors = new() {};

        // ICollections can only be read
        public ICollection<Sensor> GetSensors() {
            return _sensors.OrderBy(s => s.SensorID).ToList();
        }

        public Sensor? GetSensor(int sensorId) {
            return _sensors.Where(s => s.SensorID == sensorId).FirstOrDefault();
        }

        public ICollection<Sensor> GetDeviceSensors(int deviceId)
        {
            return _sensors
                .Where(s => s.DeviceID == deviceId)
                .OrderBy(s => s.SensorID)
                .ToList();
        }

        public bool SensorExists(int sensorId) {
            return _sensors.Any(s => s.SensorID == sensorId);
        }
        
        public bool CreateSensor(Sensor sensor) {
            if (_sensors.Find(d => d.DeviceID == sensor.SensorID) != null)
            {
                throw new DuplicateNameException("Sensor with id '" + sensor.DeviceID + "' already exists");
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