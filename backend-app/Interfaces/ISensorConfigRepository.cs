using backEndApp.DTO;
using backEndApp.Models;

namespace backEndApp.Interfaces {
    public interface ISensorConfigRepository {
        ICollection<SensorConfig> GetSensorConfigs();
        ICollection<SensorConfig> GetSensorConfigs(int sensorId);
        SensorConfig? GetSensorConfig(int sensorConfigID);
        bool SensorConfigExists(int sensorId);
        bool CreateSensorConfig(SensorConfig sensorConfig);
        bool UpdateSensorConfig(SensorConfig sensorConfig);
        bool DeleteSensorConfig(SensorConfig sensorConfig);
        bool DeleteSensorConfigs(List<SensorConfig> sensorConfigs);
        bool Save();
    }
}