using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;

namespace Finomal.Application.Users.GetUser
{
    public record GetUserByIdQuery(string UserId) : IQuery<UserDetailDto>;
}
