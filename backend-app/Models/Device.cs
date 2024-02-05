using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace backEndApp.Models;
public class Device {

    // public Device() {
    //     this.Users = new HashSet<User>();
    // }

    [Key]
    public int DeviceID { get; set; }    
    public string DeviceName { get; set; }
    public string DeviceType { get; set; }
    public string ZipCode { get; set; }

    public ICollection<Sensor> Sensors { get; set; }

    // public ICollection<User> Users { get; set; }
    public ICollection<UserDevice> UserDevices { get; set; }
}