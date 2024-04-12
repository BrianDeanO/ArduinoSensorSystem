using Microsoft.EntityFrameworkCore;
namespace backEndApp.DTO;

// Making the composite key
[PrimaryKey(nameof(UserID), nameof(DeviceID))]
public class UserDeviceDTO {
    public int UserID { get; set; }
    public int DeviceID { get; set; }
}