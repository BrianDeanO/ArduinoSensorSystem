using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace backEndApp.DTO;

public class UserDTO {
    public int UserID { get; set; }
    public string UserType { get; set; }
    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }
    public string UserPassword { get; set; }
    public bool UserIsDeleted { get; set; }
    public string UserEmail { get; set; }
    public string UserPhone { get; set; }
    public bool UserNotifications { get; set; }
}