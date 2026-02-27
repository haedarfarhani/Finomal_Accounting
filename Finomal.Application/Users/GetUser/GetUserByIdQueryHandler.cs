using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;
using Finomal.Domain.Users;

namespace Finomal.Application.Users.GetUser
{
    public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDetailDto>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result<UserDetailDto>> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(request.UserId, out var guid))
                return Result.Fail<UserDetailDto>("شناسه کاربر نامعتبر است.");

            var user = await _userRepository.GetByIdAsync(guid);
            if (user is null)
                return Result.Fail<UserDetailDto>("کاربر پیدا نشد.");

            // آخرین ۲۰ ورود
            var loginHistory = await _userRepository
                .GetLoginHistoryByUserIdAsync(guid, take: 20);

            var dto = new UserDetailDto(
                //Id: user.Id.ToString(),
                //UserName: user.UserName,
                //Email: user.Email,
                //EmailConfirmed: user.EmailConfirmed,
                //PhoneNumber: user.PhoneNumber,
                //PhoneNumberConfirmed: user.PhoneNumberConfirmed,
                //FullName: $"{user.FirstName} {user.LastName}".Trim(),
                //FirstName: user.FirstName,
                //LastName: user.LastName,
                //Roles: user.UserRoles
                //    .Where(ur => ur.Role is not null)
                //    .Select(ur => new RoleDto(
                //        Id: ur.Role!.Id,
                //        Name: ur.Role.Name,
                //        Description: ur.Role.Description))
                //    .ToList()
                //    .AsReadOnly(),
                //IsActive: user.IsActive,
                //IsLockedOut: user.IsLockedOut || (user.LockoutEndDate != null && user.LockoutEndDate > DateTime.UtcNow),
                //LockoutEndDate: user.LockoutEndDate,
                //CreatedDate: user.CreatedDate,
                //LastLoginDate: user.LastLoginDate
                //RecentLogins: loginHistory.Select(lh => new LoginHistoryDto(
                //    Id: lh.Id.ToString(),
                //    EventTime: lh.EventTime,
                //    IsSuccess: lh.IsSuccess,
                //    IpAddress: lh.IpAddress,
                //    UserAgent: lh.UserAgent,
                //    FailReason: lh.FailReason
                //)).ToList().AsReadOnly()
            );

            return Result.OK(dto);
        }
    }
}
