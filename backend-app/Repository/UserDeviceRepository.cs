using backEndApp.DTO;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;

namespace backEndApp.Repository {

    public class UserDeviceRepository: IUserDeviceRepository {
        private readonly SensorSystemContext _context;

        public UserDeviceRepository(SensorSystemContext context) {
            _context = context;
        }

        public ICollection<UserDevice> GetUserDevices() {
            return _context.UserDevices.OrderBy(ud => ud.UserID).ToList();
        }

        public UserDevice? GetUserDevice(int userId, int deviceId) {
            return _context.UserDevices
                .Where(ud => ((ud.UserID == userId) && (ud.DeviceID == deviceId))).FirstOrDefault();
        }

        public ICollection<UserDevice> GetUserDevices(int userId) {
            return _context.UserDevices
                .Where(ud => ud.UserID == userId)
                .OrderBy(ud => ud.DeviceID)
                .ToList();
        }

        public ICollection<UserDevice> GetDeviceUsers(int deviceId) {
            return _context.UserDevices
                .Where(ud => ud.DeviceID == deviceId)
                .OrderBy(ud => ud.DeviceID)
                .ToList();
        }

        public bool UserDeviceExists(int userId, int deviceId) {
            return _context.UserDevices
                .Any(ud => ((ud.UserID == userId) && (ud.DeviceID == deviceId)));
        }
        
        public bool CreateUserDevice(UserDevice userDevice) {
            _context.Add(userDevice);
            return Save();
        }

        public bool DeleteUserDevice(UserDevice userDevice) {
            _context.Remove(userDevice);
            return Save();
        }

        public bool DeleteUserDevices(List<UserDevice> userDevices) {
            _context.RemoveRange(userDevices);
            return Save();
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return (saved  > 0) ? true : false;
        }
    }
}