using backEndApp.DTO;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;

namespace backEndApp.Repository {

    public class LocalUserDeviceRepository: IUserDeviceRepository {
        private static List<UserDevice> _userDevices = new() {};

        public ICollection<UserDevice> GetUserDevices() {
            return _userDevices.OrderBy(ud => ud.UserID).ToList();
        }

        public UserDevice? GetUserDevice(int userId, int deviceId) {
            return _userDevices
                .Where(ud => (ud.UserID == userId) && (ud.DeviceID == deviceId)).FirstOrDefault();
        }

        public ICollection<UserDevice> GetUserDevices(int userId) {
            return _userDevices
                .Where(ud => ud.UserID == userId)
                .OrderBy(ud => ud.DeviceID)
                .ToList();
        }

        public ICollection<UserDevice> GetDeviceUsers(int deviceId) {
            return _userDevices
                .Where(ud => ud.DeviceID == deviceId)
                .OrderBy(ud => ud.DeviceID)
                .ToList();
        }

        public bool UserDeviceExists(int userId, int deviceId) {
            return _userDevices
                .Any(ud => ((ud.UserID == userId) && (ud.DeviceID == deviceId)));
        }
        
        public bool CreateUserDevice(UserDevice userDevice) {
            _userDevices.Add(userDevice);
            return Save();
        }

        public bool DeleteUserDevice(UserDevice userDevice) {
            return _userDevices.Remove(userDevice);
        }

        public bool DeleteUserDevices(List<UserDevice> userDevices) {
            foreach (var userDevice in userDevices) {
                _userDevices.Remove(userDevice);
            }
            return Save();
        }

        public bool Save() {
            return true;
        }
    }
}