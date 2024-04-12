using System.ComponentModel.DataAnnotations;

namespace backEndApp.Models;
public class SensorConfig {
    [Key]
    public int SensorConfigID { get; set; }  
    public required string SensorConfigKey { get; set; }
    public required string SensorConfigValue { get; set; }

    // The SensorID and Sensor object for the ONE sensor 
    public int SensorID { get; set; }
    public Sensor? Sensor { get; set; }
}