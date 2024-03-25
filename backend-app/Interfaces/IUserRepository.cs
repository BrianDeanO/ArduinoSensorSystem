using backEndApp.DTO;
using backEndApp.Models;

namespace backEndApp.Interfaces {

    public interface IUserRepository {
        ICollection<User> GetUsers();
        User? GetUser(int userId);
        User? GetUserWithLogin(string userFirstName, string userLastName, string userPassword);
        ICollection<User> GetDeviceUsers(int deviceId);
        bool UserExistsWithLogin(int userID, string userFirstName, string userLastName, string userPassword);
        ICollection<User> GetAdminUsers();
        bool UserExists(int userId);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
    }
}