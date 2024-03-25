using backEndApp.DTO;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;

namespace backEndApp.Repository {

    public class SensorDataRepository: ISensorDataRepository {
        private readonly SensorSystemContext _context;

        public SensorDataRepository(SensorSystemContext context) {
            _context = context;
        }

        public SensorData? GetSensorData(int sensorDataId) {
            return _context.SensorDatas.Find(sensorDataId);
        }

        public ICollection<SensorData> GetSensorDatas() {
            return _context.SensorDatas.OrderBy(sd => sd.SensorDataID).ToList();
        }

        public bool SensorDataExists(int sensorDataId) {
            return _context.SensorDatas.Any(sd => sd.SensorDataID == sensorDataId);
        }

        public ICollection<SensorData> GetSensorDatas(int sensorId) {
            return _context.SensorDatas
                .Where(sd => sd.SensorID == sensorId)
                .OrderBy(sd => sd.SensorDataID)
                .ToList();
        }

        public bool CreateSensorData(SensorData sensorData) {
            _context.Add(sensorData);
            return Save();
        }

        public bool UpdateSensorData(SensorData sensorData) {
            _context.Update(sensorData);
            return Save();
        }
        
        public bool DeleteSensorData(SensorData sensorData) {
            _context.Remove(sensorData);
            return Save();
        }
        
        public bool DeleteSensorDatas(List<SensorData> sensorDatas) {
            _context.RemoveRange(sensorDatas);
            return Save();
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return (saved  > 0) ? true : false;
        }
    }
}