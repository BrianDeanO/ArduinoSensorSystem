using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace backEndApp.Models;
public class TemperatureItem {
    [Key]
    public int TemperatureID { get; set; }
    public int TemperatureValue { get; set; }
    public char TemperatureUnit { get; set; }
    public string TemperatureTime { get; set; }
}