using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace backEndApp.Models;
public class DeviceDTO {
    [Key]
    public int DeviceID { get; set; }    
    public string DeviceName { get; set; }
    public string DeviceType { get; set; }
    public string ZipCode { get; set; }
}