using Finomal.Application.Abstractions;
using Finomal.Application.Users.Dtos;
using Finomal.Domain.Abstractions;
using Mapster;

namespace Finomal.Application.Users.GetRoles
{
    public class GetRolesQueryHandler : IQueryHandler<GetRolesQuery, List<RoleDto>>
    {
        private readonly IUserService _userService;
        public GetRolesQueryHandler(IUserService userService)
        {

            _userService = userService;
        }
        public async Task<Result<List<RoleDto>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            // 1. دریافت تمامی نقش‌ها از دیتابیس
            var roles = await _userService.GetAllRolesAsync();

            // 2. بررسی اینکه آیا نقشی یافت شده است
            if (roles == null || !roles.Any())
            {
                return Result.Fail<List<RoleDto>>("نقشی یافت نشد.");
            }

            // 3. نگاشت Entity های Role به RoleDto با استفاده از Mapster
            var roleDtos = roles.Adapt<List<RoleDto>>();

            // 4. برگرداندن پاسخ موفقیت‌آمیز
            return Result.OK<List<RoleDto>>(roleDtos);
        }
    }
}
