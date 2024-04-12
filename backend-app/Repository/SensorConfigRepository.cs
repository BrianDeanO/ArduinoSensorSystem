using backEndApp.DTO;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;

namespace backEndApp.Repository {
    public class SensorConfigRepository: ISensorConfigRepository {
        private readonly SensorSystemContext _context;

        public SensorConfigRepository(SensorSystemContext context) {
            _context = context;
        }
        public ICollection<SensorConfig> GetSensorConfigs() {
            return _context.SensorConfigs.OrderBy(s => s.SensorConfigID).ToList();
        }
        public ICollection<SensorConfig> GetSensorConfigs(int sensorId) {
            return _context.SensorConfigs.Where(s => s.SensorID == sensorId).OrderBy(s => s.SensorConfigID).ToList();
        }
        public SensorConfig? GetSensorConfig(int sensorConfigID) {
            return _context.SensorConfigs.Where(sc => sc.SensorConfigID == sensorConfigID).FirstOrDefault();
        }
        public bool SensorConfigExists(int sensorConfigID) {
            return _context.SensorConfigs.Any(s => s.SensorConfigID == sensorConfigID);
        }
        
        public bool CreateSensorConfig(SensorConfig sensorConfig) {
            _context.Add(sensorConfig);
            return Save();
        }

        public bool UpdateSensorConfig(SensorConfig sensorConfig) {
            _context.Update(sensorConfig);
            return Save();
        }

        public bool DeleteSensorConfig(SensorConfig sensorConfig) {
            _context.Remove(sensorConfig);
            return Save();
        }

        public bool DeleteSensorConfigs(List<SensorConfig> sensorConfigs) {
            _context.RemoveRange(sensorConfigs);
            return Save();
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return (saved  > 0) ? true : false;
        }
    }
}