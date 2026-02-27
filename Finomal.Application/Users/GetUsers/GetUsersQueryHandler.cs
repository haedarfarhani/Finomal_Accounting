using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Users;

namespace Finomal.Application.Users.GetUsers
{
    public class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, List<UserDto>>
    {
        private readonly IUserRepository _userRepository;

        public GetUsersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<List<UserDto>>> Handle(
            GetUsersQuery request,
            CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync();

            var dto = users.Select(MapToDto).ToList();

            return Result.OK(dto);
        }

        internal static UserDto MapToDto(User u) => new(
            Id: u.Id.ToString(),
            UserName: u.UserName,
            Email: u.Email,
            EmailConfirmed: u.EmailConfirmed,
            PhoneNumber: u.PhoneNumber,
            PhoneNumberConfirmed: u.PhoneNumberConfirmed,
            FullName: $"{u.FirstName} {u.LastName}".Trim(),
            FirstName: u.FirstName,
            LastName: u.LastName,
            Roles: u.UserRoles
                .Select(ur => ur.Role?.Name ?? "")
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .ToList()
                .AsReadOnly(),
            IsActive: u.IsActive,
            IsLockedOut: u.IsLockedOut || (u.LockoutEndDate != null && u.LockoutEndDate > DateTime.UtcNow),
            CreatedDate: u.CreatedDate,
            LastLoginDate: u.LastLoginDate
        );
    }
}