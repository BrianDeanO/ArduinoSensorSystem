using backEndApp.DTO;
using backEndApp.Models;
using backEndApp.Data;
using backEndApp.Interfaces;
using System.Data;

namespace backEndApp.Repository {

    public class LocalUserRepository: IUserRepository {
        private static List<User> _users = new() {};
        private static int _lastUserId = 0;
        private IUserDeviceRepository _userDeviceRepository;

        public LocalUserRepository(IUserDeviceRepository userDeviceRepository) {
            _userDeviceRepository = userDeviceRepository;
        }

        // ICollections can only be read
        public ICollection<User> GetUsers() {
            return _users.OrderBy(u => u.UserID).ToList();
        }

        public User? GetUser(int userId) {
            return _users.Where(u => u.UserID == userId).FirstOrDefault();
        }

        public ICollection<User> GetAdminUsers() {
            return _users.Where(ud => ud.UserType == "ADMIN").Select(u => u).ToList();
        }

        public ICollection<User> GetDeviceUsers(int deviceId)
        {
            var userDevices = _userDeviceRepository.GetDeviceUsers(deviceId);
            return _users.Where(u => userDevices.Any(ud => ud.UserID == u.UserID)).ToList();
        }

        public User? GetUserWithLogin(string userFirstName, string userLastName, string userPassword) {
            return _users.Where(ud => (
                ud.UserFirstName == userFirstName && 
                ud.UserLastName == userLastName &&
                ud.UserPassword == userPassword
            )).FirstOrDefault();
        }

        public bool UserExistsWithLogin(int userID, string userFirstName, string userLastName, string userPassword) {
            var tempUser = _users.Where(ud => (
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
            return _users.Any(d => d.UserID == userId); 
        }
        
        public bool CreateUser(User user) {
            user.UserID = _lastUserId++;
            if (_users.Find(d => d.UserID == user.UserID) != null)
            {
                throw new DuplicateNameException("Device with id '" + user.UserID + "' already exists");
            }
            _users.Add(user);
            return Save();
        }

        public bool UpdateUser(User user) {
            var old = _users.Find(u => u.UserID == user.UserID);
            if (old == null) {
                return false;
            }

            _users.Remove(old);
            _users.Add(user);
            return Save();
        }
        
        public bool DeleteUser(User user) {
            return _users.Remove(user);
        }

        public bool Save() {
            return true;
        }
    }
}