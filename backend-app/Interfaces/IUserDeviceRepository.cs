using backEndApp.DTO;
using backEndApp.Models;

namespace backEndApp.Interfaces {

    public interface IUserDeviceRepository {
        ICollection<UserDevice> GetUserDevices();
        UserDevice GetUserDevice(int userId, int deviceId);
        User GetUserDeviceUser(int userId);
        Device GetUserDeviceDevice(int deviceId);
        ICollection<User> GetUserDeviceUsers(int userId);
        ICollection<Device> GetUserDeviceDevices(int deviceId);
        bool UserDeviceExists(int userId, int deviceId);
        bool UserDeviceExistsWithUserID(int userId);
        bool UserDeviceExistsWithDeviceID(int deviceId);
        bool CreateUserDevice(UserDevice userDevice);
        bool UpdateUserDevice(UserDevice userDevice);
        bool DeleteUserDevice(UserDevice userDevice);
        bool DeleteUserDevices(List<UserDevice> userDevices);
        bool Save();
    }
}