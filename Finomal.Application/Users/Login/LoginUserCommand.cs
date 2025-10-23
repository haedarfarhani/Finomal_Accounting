using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;

namespace Finomal.Application.Users.Login
{
    public class LoginCommand : ICommand<LoginDto>
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required int RoleId { get; set; }
    }
}
