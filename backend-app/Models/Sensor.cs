using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace backEndApp.Models;

[Index(nameof(SensorIdent), IsUnique = true)]
public class Sensor {
    [Key]
    public int SensorID { get; set; }    
    [Required]
    public required string SensorIdent { get; set; }
    public string? SensorName { get; set; }
    public string? SensorType { get; set; }
    public int ChannelCount { get; set; }
    public bool SensorIsDeleted { get; set; }

    // The DeviceID and Device object for the ONE device 
    public int DeviceID { get; set; }
    public Device? Device { get; set; }

    // The relationship to MANY sensorDatas
    public ICollection<SensorData>? SensorDatas { get; set; }
    
    // The relationship to MANY sensorConfigs
    public ICollection<SensorConfig>? SensorConfigs { get; set; }
}