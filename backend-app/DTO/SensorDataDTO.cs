using System.Text.Json.Serialization;
namespace backEndApp.DTO;
public class SensorDataDTO {
    public int SensorDataID { get; set; }    
    public int ChannelID { get; set; }
    public float DataValue { get; set; }
    public required string DataUnit { get; set; }
    
    [JsonConverter(typeof(UnixTimeJsonConverter))]
    public DateTime TimeRecorded { get; set; }
    
    // Sensor ID Foreign Key
    public int SensorID { get; set; }
}