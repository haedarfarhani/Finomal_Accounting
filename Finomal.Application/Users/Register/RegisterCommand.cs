using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;

namespace Finomal.Application.Users.Register
{
    // فرض می‌کنیم ICommand<Result<RegisterResponseDto>> را برمی‌گرداند.
    // RegisterResponseDto می‌تواند شامل UserId و پیام موفقیت باشد.
    public class RegisterCommand : ICommand<RegisterResponseDto>
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required Guid RoleId { get; set; }
    }
}
