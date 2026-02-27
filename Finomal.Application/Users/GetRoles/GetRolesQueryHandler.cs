using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Users;

namespace Finomal.Application.Users.GetRoles
{
    public class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, List<RoleDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetRolesQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _userRepository.GetAllRolesAsync();

            var dto = roles.Select(r => new RoleDto(
                r.Id.ToString(),
                r.Name,
                r.Description
            )).ToList();

            return Result.OK(dto);
        }
    }
}
