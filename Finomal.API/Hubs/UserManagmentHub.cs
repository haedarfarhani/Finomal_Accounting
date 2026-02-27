using Finomal.Application.Users.Commands;
using Finomal.Application.Users.CreateUser;
using Finomal.Application.Users.DeleteUser;
using Finomal.Application.Users.Dtos;
using Finomal.Application.Users.GetRoles;
using Finomal.Application.Users.GetUser;
using Finomal.Application.Users.GetUsers;
using Finomal.Application.Users.ResetPassword;
using Finomal.Application.Users.UpdateUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.SignalR;

namespace Finomal.API.Hubs
{
    /// <summary>
    /// Hub مدیریت کاربران — همه متدها نیاز به احراز هویت دارند.
    /// کلاینت Vue با این Hub از طریق SignalR ارتباط می‌گیرد.
    ///
    /// رویدادهایی که به کلاینت push می‌شوند:
    ///   ReceiveUsers          → لیست کامل UserDto[]
    ///   ReceiveRoles          → لیست RoleDto[]
    ///   UserCreated           → UserDto
    ///   UserUpdated           → UserDto
    ///   UserStatusUpdated     → { userId, isActive }
    ///   UserDeleted           → userId (string)
    ///   ReceiveError          → پیام خطا (string)
    /// </summary>
    [Authorize]
    public class UserManagementHub : Hub
    {
        private readonly ISender _mediator;
        private readonly ILogger<UserManagementHub> _logger;

        public UserManagementHub(ISender mediator, ILogger<UserManagementHub> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        // ──────────────────────────────────────────────────────
        //  اتصال / قطع اتصال
        // ──────────────────────────────────────────────────────

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("UserManagementHub: کاربر {ConnectionId} متصل شد.", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("UserManagementHub: کاربر {ConnectionId} قطع شد.", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        // ──────────────────────────────────────────────────────
        //  Query Methods  ─  خواندن داده‌ها
        // ──────────────────────────────────────────────────────

        /// <summary>
        /// دریافت لیست کامل کاربران — فقط به caller برمی‌گردد.
        /// کلاینت: await connection.invoke("GetUsers")
        /// </summary>
        public async Task GetUsers()
        {
            try
            {
                var result = await _mediator.Send(new GetUsersQuery());
                if (result.Success)
                    await Clients.Caller.SendAsync("ReceiveUsers", result.Value);
                else
                    await SendErrorAsync(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در GetUsers");
                await SendErrorAsync("خطا در دریافت لیست کاربران.");
            }
        }

        /// <summary>
        /// دریافت جزئیات کامل یک کاربر همراه تاریخچه ورود.
        /// کلاینت: await connection.invoke("GetUserById", userId)
        /// رویداد: ReceiveUserDetail
        /// </summary>
        public async Task GetUserById(string userId)
        {
            try
            {
                var result = await _mediator.Send(new GetUserByIdQuery(userId));
                if (result.Success)
                    await Clients.Caller.SendAsync("ReceiveUserDetail", result.Value);
                else
                    await SendErrorAsync(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در GetUserById ({UserId})", userId);
                await SendErrorAsync("خطا در دریافت اطلاعات کاربر.");
            }
        }

        /// <summary>
        /// دریافت لیست همه نقش‌ها.
        /// کلاینت: await connection.invoke("GetRoles")
        /// رویداد: ReceiveRoles
        /// </summary>
        public async Task GetRoles()
        {
            try
            {
                var result = await _mediator.Send(new GetRolesQuery());
                if (result.Success)
                    await Clients.Caller.SendAsync("ReceiveRoles", result.Value);
                else
                    await SendErrorAsync(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در GetRoles");
                await SendErrorAsync("خطا در دریافت نقش‌ها.");
            }
        }

        // ──────────────────────────────────────────────────────
        //  Command Methods  ─  تغییر داده‌ها
        // ──────────────────────────────────────────────────────

        /// <summary>
        /// ایجاد کاربر جدید — پس از موفقیت به همه کلاینت‌ها اطلاع می‌دهد.
        /// کلاینت: await connection.invoke("CreateUser", request)
        /// رویداد broadcast: UserCreated
        /// </summary>
        public async Task CreateUser(CreateUserRequest request)
        {
            try
            {
                var command = new CreateUserCommand(
                    request.FirstName,
                    request.LastName,
                    request.UserName,
                    request.Email,
                    request.PhoneNumber,
                    request.Password,
                    request.RoleId);

                var result = await _mediator.Send(command);

                if (!result.Success)
                {
                    await SendErrorAsync(result.Error);
                    return;
                }

                // دریافت اطلاعات کاربر تازه‌ساخته برای broadcast
                var user = await _mediator.Send(
                    new GetUsersQuery()); // یا GetUserByUserNameQuery اگر داشتید

                // ساده‌ترین راه: لیست کامل را refresh کنیم
                await BroadcastUserListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در CreateUser");
                await SendErrorAsync("خطا در ایجاد کاربر.");
            }
        }

        /// <summary>
        /// ویرایش اطلاعات کاربر — broadcast به همه.
        /// کلاینت: await connection.invoke("UpdateUser", request)
        /// رویداد broadcast: UserUpdated → UserDto
        /// </summary>
        public async Task UpdateUser(UpdateUserRequest request)
        {
            try
            {
                var command = new UpdateUserCommand(
                    request.UserId,
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.PhoneNumber,
                    request.RoleId);

                var result = await _mediator.Send(command);

                if (!result.Success)
                {
                    await SendErrorAsync(result.Error);
                    return;
                }

                // دریافت داده به‌روز و ارسال به همه
                var userResult = await _mediator.Send(new GetUserByIdQuery(request.UserId));
                if (userResult.Success)
                    await Clients.All.SendAsync("UserUpdated", userResult.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در UpdateUser ({UserId})", request.UserId);
                await SendErrorAsync("خطا در ویرایش کاربر.");
            }
        }

        /// <summary>
        /// تغییر وضعیت فعال/غیرفعال — broadcast به همه.
        /// کلاینت: await connection.invoke("ToggleUserStatus", userId, isActive)
        /// رویداد broadcast: UserStatusUpdated → { userId, isActive }
        /// </summary>
        public async Task ToggleUserStatus(string userId, bool isActive)
        {
            try
            {
                var result = await _mediator.Send(new ToggleUserActiveCommand(userId, isActive));

                if (!result.Success)
                {
                    await SendErrorAsync(result.Error);
                    return;
                }

                await Clients.All.SendAsync("UserStatusUpdated", new { userId, isActive });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ToggleUserStatus ({UserId})", userId);
                await SendErrorAsync("خطا در تغییر وضعیت کاربر.");
            }
        }

        /// <summary>
        /// حذف نرم کاربر — broadcast به همه.
        /// کلاینت: await connection.invoke("DeleteUser", userId)
        /// رویداد broadcast: UserDeleted → userId
        /// </summary>
        public async Task DeleteUser(string userId)
        {
            try
            {
                var result = await _mediator.Send(new DeleteUserCommand(userId));

                if (!result.Success)
                {
                    await SendErrorAsync(result.Error);
                    return;
                }

                await Clients.All.SendAsync("UserDeleted", userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در DeleteUser ({UserId})", userId);
                await SendErrorAsync("خطا در حذف کاربر.");
            }
        }

        /// <summary>
        /// ریست رمز عبور توسط ادمین.
        /// کلاینت: await connection.invoke("ResetPassword", request)
        /// رویداد به caller: PasswordResetSuccess
        /// </summary>
        public async Task ResetPassword(Application.Users.Dtos.ResetPasswordRequest request)
        {
            try
            {
                var result = await _mediator.Send(
                    new ResetPasswordCommand(request.UserId, request.NewPassword));

                if (result.Success)
                    await Clients.Caller.SendAsync("PasswordResetSuccess", request.UserId);
                else
                    await SendErrorAsync(result.Error);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "خطا در ResetPassword ({UserId})", request.UserId);
                await SendErrorAsync("خطا در ریست رمز عبور.");
            }
        }

        // ──────────────────────────────────────────────────────
        //  Private Helpers
        // ──────────────────────────────────────────────────────

        /// <summary>
        /// لیست کاربران را دوباره واکشی کرده و به همه broadcast می‌کند.
        /// </summary>
        private async Task BroadcastUserListAsync()
        {
            var result = await _mediator.Send(new GetUsersQuery());
            if (result.Success)
                await Clients.All.SendAsync("ReceiveUsers", result.Value);
        }

        /// <summary>
        /// ارسال پیام خطا فقط به caller.
        /// </summary>
        private Task SendErrorAsync(string message)
            => Clients.Caller.SendAsync("ReceiveError", message);
    }
}