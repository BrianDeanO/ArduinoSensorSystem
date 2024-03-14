using backEndApp.DTO;
using backEndApp.Models;

namespace backEndApp.Interfaces {

    public interface IUserRepository {
        ICollection<User> GetUsers();
        User GetUser(int userId);
        User GetUserWithLogin(string userFirstName, string userLastName, string userPassword);
        bool UserExistsWithLogin(int userID, string userFirstName, string userLastName, string userPassword);
        ICollection<UserDevice> GetUserDevices(int userId);
        ICollection<Device> GetDevicesFromUser(int userId);
        ICollection<User> GetAdminUsers();
        bool UserExists(int userId);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
    }
}