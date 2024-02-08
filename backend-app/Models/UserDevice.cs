using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace backEndApp.Models;

// Making the composite key
[PrimaryKey(nameof(UserID), nameof(DeviceID))]
public class UserDevice {
    // The ID's and objects for the associated User and Device
    public int UserID { get; set; }
    public int DeviceID { get; set; }
    
    public User User { get; set; }
    public Device Device { get; set; }
}