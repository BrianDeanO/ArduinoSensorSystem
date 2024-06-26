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
        
        public ICollection<User> GetUsers() {
            return _context.Users.OrderBy(u => u.UserID).ToList();
        }

        public User? GetUser(int userId) {
            return _context.Users.Where(u => u.UserID == userId).FirstOrDefault();
        }

        public ICollection<User> GetDeviceUsers(int deviceId)
        {
            var userDevices = _context.UserDevices.Where(ud => ud.DeviceID == deviceId).ToList();
            return _context.Users.Where(u => userDevices.Any(ud => ud.UserID == u.UserID)).ToList();
        }

        public ICollection<User> GetAdminUsers() {
            return _context.Users.Where(ud => ud.UserType == "ADMIN").Select(u => u).ToList();
        }

        public User? GetUserWithLogin(string userFirstName, string userLastName, string userPassword) {
            return _context.Users.Where(ud => (
                ud.UserFirstName == userFirstName && 
                ud.UserLastName == userLastName &&
                ud.UserPassword == userPassword
            )).FirstOrDefault();
        }

        public bool UserExistsWithLogin(int userID, string userFirstName, string userLastName, string userPassword) {
            var tempUser = _context.Users.Where(ud => (
                ud.UserFirstName == userFirstName && 
                ud.UserLastName == userLastName &&
                ud.UserPassword == userPassword
            )).FirstOrDefault();

            if((tempUser != null) && (tempUser.UserID != userID)) {
                return true;
            } else {
                return false;
            }
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