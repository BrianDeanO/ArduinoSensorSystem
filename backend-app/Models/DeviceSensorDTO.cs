using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace backEndApp.Models;

// Making the composite key
[PrimaryKey(nameof(DeviceID), nameof(SensorID))]
public class DeviceSensorDTO {
    public int DeviceID { get; set; }
    public int SensorID { get; set; }
}