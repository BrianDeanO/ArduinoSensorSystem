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

        // ICollections can only be read
        public ICollection<UserDevice> GetUserDevices() {
            return _context.UserDevices.OrderBy(ud => ud.UserID).ToList();
        }

        public UserDevice GetUserDevice(int userId, int deviceId) {
            return _context.UserDevices
                .Where(ud => ((ud.UserID == userId) && (ud.DeviceID == deviceId))).FirstOrDefault();
        }

        public User GetUserDeviceUser(int userId) {
            return _context.Users.Where(u => u.UserID == userId).FirstOrDefault();
        }

        public Device GetUserDeviceDevice(int deviceId) {
            return _context.Devices.Where(d => d.DeviceID == deviceId).FirstOrDefault();
        }

        public bool UserDeviceExists(int userId, int deviceId) {
            return _context.UserDevices
                .Any(ud => ((ud.UserID == userId) && (ud.DeviceID == deviceId)));
        }

        public bool UserDeviceExistsWithUserID(int userId) {
            return _context.UserDevices.Any(ud => ud.UserID == userId);
        }

        public bool UserDeviceExistsWithDeviceID(int deviceId) {
            return _context.UserDevices.Any(ud => ud.DeviceID == deviceId);
        }
        
        public bool CreateUserDevice(UserDevice userDevice) {
            _context.Add(userDevice);
            return Save();
        }

        public bool UpdateUserDevice(UserDevice userDevice) {
            _context.Update(userDevice);
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