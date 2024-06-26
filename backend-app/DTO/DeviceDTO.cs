namespace backEndApp.DTO;

public class DeviceDTO {
    public int DeviceID { get; set; }    
    public string? DeviceIdent { get; set; }    
    public string? DeviceName { get; set; }
    public string? DeviceType { get; set; }
    public string? DeviceZipCode { get; set; }
    public DateTime? DeviceLastSeen { get; set; }
    public bool? DeviceIsDeleted { get; set; }

    // Device Configuration Fields
    public int? DeviceUpdateInterval { get; set; }
}