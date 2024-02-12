using backEndApp.DTO;
using backEndApp.Models;

namespace backEndApp.Interfaces {

    public interface IUserRepository {
        ICollection<User> GetUsers();
        User GetUser(int userId);
        ICollection<UserDevice> GetUserDevices(int userId);
        ICollection<Device> GetDevicesFromUser(int userId);
        /*
                DO WE WANT A GetUserSensors? Probably not needed...
        */
        bool UserExists(int userId);
        bool CreateUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        bool Save();
    }
}