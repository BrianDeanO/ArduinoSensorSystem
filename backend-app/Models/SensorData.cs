using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace backEndApp.Models;
public class SensorData {
    [Key]
    public int SensorDataID { get; set; }    
    public int ChannelID { get; set; }
    public float DataValue { get; set; }
    public string DataUnit { get; set; }

    [JsonConverter(typeof(UnixTimeJsonConverter))]
    public DateTime TimeRecorded { get; set; }

    // // Sensor ID Foreign Key
    public int SensorID { get; set; }
    // public SensorData Sensor { get; set; } // Definitely keep this in
}