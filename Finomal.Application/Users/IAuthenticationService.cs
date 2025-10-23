using Finomal.Application.Users.Dtos;
using Finomal.Domain.Users;

namespace Finomal.Application.Users
{
    public interface IAuthenticationService
    {
        string GeneratePasswordHash(string password);
        bool VerifyPassword(string password, string passwordHash);
        Task<LoginDto> RegisterUserAsync(string username, string email, string password, string firstName = null, string lastName = null);
        Task<LoginDto> LoginUserAsync(string username, string password,int role);
        Task LogoutUserAsync(int userId); 
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(int userId, string newPassword); 
    }
}
