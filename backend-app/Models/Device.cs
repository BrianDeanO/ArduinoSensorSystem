using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace backEndApp.Models;

[Index(nameof(DeviceIdent), IsUnique = true)]
public class Device {
    [Key]
    public int DeviceID { get; set; }    

    [Required]
    public string DeviceIdent { get; set; }
    public string DeviceName { get; set; }
    public string DeviceType { get; set; }
    public string ZipCode { get; set; }
}