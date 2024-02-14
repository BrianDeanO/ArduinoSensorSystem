using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace backEndApp.Models;

[Index(nameof(SensorIdent), IsUnique = true)]
public class Sensor {
    [Key]
    public int SensorID { get; set; }    
    [Required]
    public string SensorIdent { get; set; }
    public string? SensorName { get; set; }
    public string? SensorType { get; set; }
    public int ChannelCount { get; set; }

    // The DeviceID and Device object for the ONE device 
    public int DeviceID { get; set; }
    public Device Device { get; set; }

    // The relationship to MANY sensorDatas
    public ICollection<SensorData> SensorDatas { get; set; }
}