using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Application.Users.GetUsers;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Users;

namespace Finomal.Application.Users.GetUsersRole
{
    public class GetUsersByRoleQueryHandler : IQueryHandler<GetUsersByRoleQuery, List<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersByRoleQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<List<UserDto>>> Handle(
            GetUsersByRoleQuery request,
            CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.RoleId, out var roleGuid))
                return Result.Fail<List<UserDto>>("شناسه نقش نامعتبر است.");

            var users = await _userRepository.GetByRoleIdAsync(roleGuid);

            return Result.OK(users.Select(GetUsersQueryHandler.MapToDto).ToList());
        }
    }
}
