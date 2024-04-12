namespace backEndApp.DTO;

public class UserDTO {
    public int UserID { get; set; }
    public required string UserType { get; set; }
    public required string UserFirstName { get; set; }
    public required string UserLastName { get; set; }
    public required string UserPassword { get; set; }
    public bool UserIsDeleted { get; set; }
    public string? UserEmail { get; set; }
    public string? UserPhone { get; set; }
    public bool UserNotifications { get; set; }
}