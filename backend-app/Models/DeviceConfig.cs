using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace backEndApp.Models;

public class DeviceConfig {
    // Device Configuration Fields
    public string DevicePollingInterval { get; set; }
}