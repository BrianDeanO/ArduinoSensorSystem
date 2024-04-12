using backEndApp.DTO;
using backEndApp.Models;

namespace backEndApp.Interfaces {

    public interface IUserDeviceRepository {
        ICollection<UserDevice> GetUserDevices();
        UserDevice? GetUserDevice(int userId, int deviceId);
        ICollection<UserDevice> GetUserDevices(int userId);
        bool UserDeviceExists(int userId, int deviceId);
        bool CreateUserDevice(UserDevice userDevice);
        bool DeleteUserDevice(UserDevice userDevice);
        bool DeleteUserDevices(List<UserDevice> userDevices);
        bool Save();
    }
}