using backEndApp.DTO;
using backEndApp.Models;

namespace backEndApp.Interfaces {

    public interface ISensorDataRepository {
        SensorData? GetSensorData(int sensorDataId);
        ICollection<SensorData> GetSensorDatas();
        ICollection<SensorData> GetSensorDatas(int sensorId);
        bool SensorDataExists(int sensorDataId);
        bool CreateSensorData(SensorData sensorData);
        bool UpdateSensorData(SensorData sensorData);
        bool DeleteSensorData(SensorData sensorData);
        bool DeleteSensorDatas(List<SensorData> sensorDatas);
        bool Save();
    }
}