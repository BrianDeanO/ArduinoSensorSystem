using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace backEndApp.DTO;
public class SensorDTO {
    // [Key]
    public int SensorID { get; set; }    
    public string SensorName { get; set; }
    public string SensorType { get; set; }
    public int ChannelCount { get; set; }
    
    public int DeviceID { get; set; }
}