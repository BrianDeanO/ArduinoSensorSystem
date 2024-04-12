using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace backEndApp.Models;

[Index(nameof(DeviceIdent), IsUnique = true)]

public class Device {
    [Key]
    public int DeviceID { get; set; }    

    [Required]
    public required string DeviceIdent { get; set; }

    // Device Configuration Fields
    
    // Interval in seconds which the device will acquire and send new data.
    public int DeviceUpdateInterval { get; set; }
    
    public string? DeviceName { get; set; }
    public string? DeviceType { get; set; }
    public string? DeviceZipCode { get; set; }
    public DateTime? DeviceLastSeen { get; set; }
    public bool DeviceIsDeleted { get; set; }

    // The relationship to MANY sensors
    public ICollection<Sensor>? Sensors { get; set; }

    // The relationship to MANY UserDevices
    public ICollection<UserDevice>? UserDevices { get; set; }
}