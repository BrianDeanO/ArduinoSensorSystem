using backEndApp.DTO;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;

namespace backEndApp.Repository {

    public class SensorRepository: ISensorRepository {
        private readonly SensorSystemContext _context;

        public SensorRepository(SensorSystemContext context) {
            _context = context;
        }
        public ICollection<Sensor> GetSensors() {
            return _context.Sensors.OrderBy(s => s.SensorID).ToList();
        }

        public ICollection<Sensor> GetDeviceSensors(int deviceId)
        {
            return _context.Sensors
                .Where(s => s.DeviceID == deviceId)
                .OrderBy(s => s.SensorID)
                .ToList();
        }

        public Sensor? GetSensor(int sensorId) {
            return _context.Sensors.Where(s => s.SensorID == sensorId).FirstOrDefault();
        }

        public ICollection<SensorData> GetSensorDatas(int sensorId) {
            return _context.SensorDatas.Where(sd => sd.SensorID == sensorId).ToList();
        }
        
        public ICollection<SensorConfig> GetSensorConfigs(int sensorId) {
            return _context.SensorConfigs.Where(sd => sd.SensorID == sensorId).ToList();
        }

        public bool SensorExists(int sensorId) {
            return _context.Sensors.Any(s => s.SensorID == sensorId);
        }
        
        public bool CreateSensorConfig(SensorConfig sensorConfig) {
            _context.Add(sensorConfig);
            return Save();
        }
        
        public bool CreateSensor(Sensor sensor) {
            _context.Add(sensor);
            return Save();
        }

        public bool UpdateSensor(Sensor sensor) {
            _context.Update(sensor);
            return Save();
        }

        public bool DeleteSensor(Sensor sensor) {
            _context.Remove(sensor);
            return Save();
        }

        public bool DeleteSensors(List<Sensor> sensors) {
            _context.RemoveRange(sensors);
            return Save();
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return (saved  > 0) ? true : false;
        }
    }
}