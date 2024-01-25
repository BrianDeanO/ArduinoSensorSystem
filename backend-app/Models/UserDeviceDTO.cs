using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace backEndApp.Models;

// Making the composite key
[PrimaryKey(nameof(UserID), nameof(DeviceID))]
public class UserDeviceDTO {
    public int UserID { get; set; }
    public int DeviceID { get; set; }
}