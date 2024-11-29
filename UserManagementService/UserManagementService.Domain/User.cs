namespace UserManagementService.Domain;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; } // Store hashed passwords
    public string Role { get; set; }
}

