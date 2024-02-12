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

        // ICollections can only be read
        public ICollection<SensorData> GetSensorDatas() {
            return _context.SensorDatas.OrderBy(sd => sd.SensorDataID).ToList();
        }

        public SensorData GetSensorData(int sensorDataId) {
            return _context.SensorDatas.Where(sd => sd.SensorDataID == sensorDataId).FirstOrDefault();
        }
        // public ICollection<SensorData> GetSensorDataInDateRange(DateTime parsedDateTime) {
        //     return _context.SensorDatas
        //         .Where(sd => (DateTime.Compare(sd.TimeRecorded, parsedDateTime) >= 0))
        //         .Select(sd => sd)
        //         .ToList();
        // }

        public Sensor GetSensorDataSensor(int sensorId) {
            // var sensorList = _context.Sensors.Where(s => (s.SensorDataID == sensorDataId)).ToList();
            return _context.Sensors.Where(s => s.SensorID == sensorId).FirstOrDefault();
        }

        public bool SensorDataExists(int sensorDataId) {
            return _context.SensorDatas.Any(sd => sd.SensorDataID == sensorDataId);
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