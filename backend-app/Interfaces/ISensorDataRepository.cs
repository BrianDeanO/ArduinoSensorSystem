using backEndApp.DTO;
using backEndApp.Models;

namespace backEndApp.Interfaces {

    public interface ISensorDataRepository {
        ICollection<SensorData> GetSensorDatas();
        bool CreateSensorData(SensorData sensorData);
        bool DeleteSensorDatas(List<SensorData> sensorDatas);
        bool Save();
    }
}