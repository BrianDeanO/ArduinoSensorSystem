using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace backEndApp.Models;
public class User {

    // public User() {
    //     this.Devices = new HashSet<Device>();
    // }

    [Key]
    public int UserID { get; set; }
    public string UserType { get; set; }
    public string UserFirstName { get; set; }
    public string UserLastName { get; set; }
    public string UserPassword { get; set; }
    public string UserEmail { get; set; }
    public string UserPhone { get; set; }
    public bool UserNotifications { get; set; }

    // public ICollection<Device> Devices { get; set; }
    public ICollection<UserDevice> UserDevices { get; set; }
}