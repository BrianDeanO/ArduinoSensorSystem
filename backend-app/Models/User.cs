using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace backEndApp.Models;
public class User {
    [Key]
    public int UserID { get; set; }
    public string UserType { get; set; }
    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }
    public string UserPassword { get; set; }
    public bool UserIsDeleted { get; set; }
    public string UserEmail { get; set; }
    public string UserPhone { get; set; }
    public bool UserNotifications { get; set; }

    // The relationship for the MANY UserDevices
    public ICollection<UserDevice> UserDevices { get; set; }
}