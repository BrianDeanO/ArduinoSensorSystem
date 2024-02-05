using backEndApp.DTO;
using backEndApp.Models;

namespace backEndApp.Interfaces {

    public interface ISensorDataRepository {
        ICollection<SensorData> GetSensorDatas();
        SensorData GetSensorData(int sensorDataId);
        Sensor GetSensorDataSensor(int sensorId);
        bool SensorDataExists(int sensorDataId);
        bool CreateSensorData(SensorData sensorData);
        bool UpdateSensorData(SensorData sensorData);
        bool DeleteSensorData(SensorData sensorData);
        bool Save();
    }
}