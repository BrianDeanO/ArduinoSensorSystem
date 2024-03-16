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

        public ICollection<SensorData> GetSensorDatas() {
            return _context.SensorDatas.OrderBy(sd => sd.SensorDataID).ToList();
        }

        public bool CreateSensorData(SensorData sensorData) {
            _context.Add(sensorData);
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