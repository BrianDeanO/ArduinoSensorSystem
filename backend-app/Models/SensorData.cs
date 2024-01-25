using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace backEndApp.Models;
public class SensorData {
    [Key]
    public int SensorDataID { get; set; }    
    public int ChannelID { get; set; }
    public float DataValue { get; set; }
    public string DataUnit { get; set; }
    public DateTime TimeRecorded { get; set; }

    // // Sensor ID Foreign Key
    public int SensorID { get; set; }
    // public SensorData Sensor { get; set; } // Definitely keep this in
}