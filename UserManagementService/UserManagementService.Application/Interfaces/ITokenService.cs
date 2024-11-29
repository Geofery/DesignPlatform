using UserManagementService.Application.DTOs;
using UserManagementService.Domain;

namespace UserManagementService.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(LoginRequest user);
}
