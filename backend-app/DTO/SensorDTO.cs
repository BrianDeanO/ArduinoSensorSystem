using backEndApp.Models;
namespace backEndApp.DTO;

public class SensorDTO {
    public int SensorID { get; set; }    
    public string? SensorIdent { get; set; }    
    public string? SensorName { get; set; }
    public string? SensorType { get; set; }
    public int? ChannelCount { get; set; }
    public bool? SensorIsDeleted { get; set; }
    
    // Device ID Foreign Key
    public int DeviceID { get; set; }
}