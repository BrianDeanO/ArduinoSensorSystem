using backEndApp.DTO;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;

namespace backEndApp.Repository {

    public class UserRepository: IUserRepository {
        private readonly SensorSystemContext _context;

        public UserRepository(SensorSystemContext context) {
            _context = context;
        }

        // ICollections can only be read
        public ICollection<User> GetUsers() {
            return _context.Users.OrderBy(u => u.UserID).ToList();
        }

        public User GetUser(int userId) {
            return _context.Users.Where(u => u.UserID == userId).FirstOrDefault();
        }

        public ICollection<UserDevice> GetUserDevices(int userId) {
            // var sensorList = _context.Sensors.Where(s => (s.UserID == userId)).ToList();
            return _context.UserDevices.Where(ud => (ud.UserID == userId)).ToList();
        }

        public ICollection<Device> GetDevicesFromUser(int userId) {
            // var sensorList = _context.Sensors.Where(s => (s.DeviceID == deviceId)).ToList();
            return _context.UserDevices.Where(ud => ud.UserID == userId).Select(ud => ud.Device).ToList();
        }

        public bool UserExists(int userId) {
            return _context.Users.Any(d => d.UserID == userId); 
        }
        
        public bool CreateUser(User user) {
            _context.Add(user);
            return Save();
        }

        public bool UpdateUser(User user) {
            _context.Update(user);
            return Save();
        }
        public bool DeleteUser(User user) {
            _context.Remove(user);
            return Save();
        }

        public bool Save() {
            var saved = _context.SaveChanges();
            return (saved  > 0) ? true : false;
        }
    }
}