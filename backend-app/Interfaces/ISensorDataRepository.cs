using backEndApp.DTO;
using backEndApp.Models;

namespace backEndApp.Interfaces {

    public interface ISensorDataRepository {
        ICollection<SensorData> GetSensorDatas();
        ICollection<SensorData> GetSensorDatas(int sensorId);
        bool CreateSensorData(SensorData sensorData);
        bool DeleteSensorDatas(List<SensorData> sensorDatas);
        bool Save();
    }
}