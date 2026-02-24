using Finomal.Application.Users.GetRoles;
using Finomal.Application.Users.Login;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Finomal.API.Hubs
{
    [AllowAnonymous]
    public class AccountingHub : Hub
    {
        private readonly ISender _mediator;

        public AccountingHub(ISender mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        public async Task Login(LoginCommand command)
        {
            
            if (string.IsNullOrWhiteSpace(command.UserName) ||
                string.IsNullOrWhiteSpace(command.Password) ||
                command.RoleId == Guid.Empty)
            {
                await Clients.Caller.SendAsync("LoginResult", new
                {
                    isSuccess = false,
                    message = "نام کاربری، رمز عبور و نقش معتبر الزامی است.",
                    errors = new[] { new { code = "INVALID_INPUT", message = "ورودی نامعتبر" } }
                });
                return;
            }

            try
            {
                var result = await _mediator.Send(command, Context.ConnectionAborted);

                if (result.Success)
                {
                    var value = result.Value;
                    await Clients.Caller.SendAsync("LoginResult", new
                    {
                        isSuccess = true,
                        data = new
                        {
                            token = value.Token,
                            userId = value.UserId,
                            username = value.UserName,
                            roleId = value.RoleId,
                            expiresIn = value.TokenExpiry,
                            refreshToken = value.RefreshToken,
                            refreshTokenExpiry = value.RefreshTokenExpiry
                        }
                    });

                    // اضافه کردن به گروه نقش
                    if (value.RoleId != null)
                    {
                        await Groups.AddToGroupAsync(Context.ConnectionId, $"Role_{value.RoleId}");
                    }
                }
                else
                {
                    await Clients.Caller.SendAsync("LoginResult", new
                    {
                        isSuccess = false,
                        message = result.Error ?? "ورود ناموفق",
                        errors = new[] { new { code = "AUTH_FAILED", message = result.Error } }
                    });
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("LoginResult", new
                {
                    isSuccess = false,
                    message = "خطای سرور در هنگام ورود",
                    errors = new[] { new { code = "SERVER_ERROR", message = ex.Message } }
                });
            }
        }

        [AllowAnonymous]
        public async Task GetAvailableRoles()
        {
            try
            {
                var query = new GetRolesQuery();
                var result = await _mediator.Send(query, Context.ConnectionAborted);

                if (result.Success)
                {
                    await Clients.Caller.SendAsync("RolesList", new
                    {
                        isSuccess = true,
                        data = result.Value
                    });
                }
                else
                {
                    await Clients.Caller.SendAsync("RolesError", new
                    {
                        isSuccess = false,
                        message = result.Error ?? "خطا در بارگذاری نقش‌ها",
                        errors = new[] { new { code = "ROLES_LOAD_FAILED", message = result.Error } }
                    });
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("RolesError", new
                {
                    isSuccess = false,
                    message = "خطای سرور در بارگذاری نقش‌ها",
                    errors = new[] { new { code = "SERVER_ERROR", message = ex.Message } }
                });
            }
        }

        public override async Task OnConnectedAsync()
        {
            // می‌تونی اینجا لاگ بزنی یا کاربر رو به گروه عمومی اضافه کنی
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // اختیاری: حذف از همه گروه‌ها
            await base.OnDisconnectedAsync(exception);
        }
    }
}