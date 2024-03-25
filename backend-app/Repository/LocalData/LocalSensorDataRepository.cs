using backEndApp.DTO;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;
using System.Data;

namespace backEndApp.Repository
{

    public class LocalSensorDataRepository : ISensorDataRepository
    {
        private static List<SensorData> _sensorData = new() { };
        private static int _lastSensorDataId = 0;

        public SensorData? GetSensorData(int sensorDataId)
        {
            return _sensorData.Find(sd => sd.SensorDataID == sensorDataId);
        }

        public ICollection<SensorData> GetSensorDatas()
        {
            return _sensorData.OrderBy(sd => sd.SensorDataID).ToList();
        }

        public ICollection<SensorData> GetSensorDatas(int sensorId)
        {
            return _sensorData
                .Where(sd => sd.SensorID == sensorId)
                .OrderBy(sd => sd.SensorDataID)
                .ToList();
        }

        public bool SensorDataExists(int sensorDataId) {
            return _sensorData.Any(sd => sd.SensorDataID == sensorDataId);
        }

        public bool CreateSensorData(SensorData sensorData)
        {
            sensorData.SensorDataID = _lastSensorDataId++;
            if (_sensorData.Find(sd => sd.SensorDataID == sensorData.SensorDataID) != null)
            {
                throw new DuplicateNameException("SensorData with id '" + sensorData.SensorDataID + "' already exists");
            }
            if (_sensorData.Find(sd => sd.TimeRecorded == sensorData.TimeRecorded && sd.ChannelID == sensorData.ChannelID) != null)
            {
                throw new DuplicateNameException("SensorData with time recorded '" + sensorData.TimeRecorded + "' and channel id '" + sensorData.ChannelID + "' already exists");
            }
            _sensorData.Add(sensorData);
            return Save();
        }

        public bool UpdateSensorData(SensorData sensorData) {
            var old = _sensorData.Find(d => d.SensorDataID == sensorData.SensorDataID);
            if (old == null)
            {
                return false;
            }

            _sensorData.Remove(old);
            _sensorData.Add(sensorData);
            return true;
        }
        
        public bool DeleteSensorData(SensorData sensorData) {
            return _sensorData.Remove(sensorData);
        }

        public bool DeleteSensorDatas(List<SensorData> sensorDatas)
        {
            foreach (var sensorData in sensorDatas)
            {
                _sensorData.Remove(sensorData);
            }
            return Save();
        }

        public bool Save()
        {
            return true;
        }
    }
}