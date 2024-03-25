using backEndApp.DTO;
using backEndApp.Models;

namespace backEndApp.Interfaces {
    public interface IDeviceRepository {
        ICollection<Device> GetDevices();
        Device? GetDevice(int deviceId);
        ICollection<Device> GetDevicesForUser(int userId);
        bool DeviceExists(int deviceId);
        bool CreateDevice(Device device);
        bool UpdateDevice(Device device);
        bool DeleteDevice(Device device);
        bool Save();
    }
}