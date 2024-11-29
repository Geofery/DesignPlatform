using UserManagementService.Domain;

public interface ITokenService
{
    string GenerateToken(User user);
}
