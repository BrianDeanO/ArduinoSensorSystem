using backEndApp.DTO;
using backEndApp.Models;

namespace backEndApp.Interfaces {

    public interface ISensorRepository {
        ICollection<Sensor> GetSensors();
        Sensor? GetSensor(int sensorId);
        ICollection<Sensor> GetDeviceSensors(int deviceId);
        bool SensorExists(int sensorId);
        bool CreateSensor(Sensor sensor);
        bool UpdateSensor(Sensor sensor);
        bool DeleteSensor(Sensor sensor);
        bool DeleteSensors(List<Sensor> sensors);
        bool Save();
    }
}