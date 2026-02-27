using Finomal.Application.Users.GetRoles;
using Finomal.Application.Users.Login;
using Finomal.Application.Users.Register;
using Finomal.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Finomal.API.Hubs
{
    [AllowAnonymous]
    public class AccountingAuthUserHub : Hub
    {
        private readonly IUserRepository _userRepository;
        private readonly ISender _mediator;

        public AccountingAuthUserHub(ISender mediator, IUserRepository userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }
        private const string AdminsGroup = "admins";
        private const string OnlineGroup = "online";
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

        /// <summary>
        /// ثبت‌نام کاربر جدید — RegisterCommand عیناً از کلاینت دریافت می‌شود
        /// </summary>
        [AllowAnonymous]
        public async Task Register(RegisterCommand command)
        {
            if (!_isRegisterInputValid(command))
            {
                await Clients.Caller.SendAsync("RegistrationResult", new
                {
                    isSuccess = false,
                    message = "همه فیلدهای ضروری باید پر شوند.",
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

                    // تأیید ثبت‌نام به خود کاربر
                    await Clients.Caller.SendAsync("RegistrationResult", new
                    {
                        isSuccess = true,
                        data = new
                        {
                            userId = value.UserId,
                            userName = value.UserName,
                            message = value.Message,        // "ثبت‌نام با موفقیت انجام شد."
                            activationPending = true                  // IsActive = false تا تأیید مدیر
                        }
                    });

                    // اطلاع‌رسانی به مدیران برای تأیید
                    await Clients.Group(AdminsGroup).SendAsync("NewUserRegistered", new
                    {
                        userId = value.UserId,
                        userName = value.UserName,
                        registeredAt = DateTime.UtcNow,
                        isActive = false,
                        requiresApproval = true
                    });
                }
                else
                {
                    await Clients.Caller.SendAsync("RegistrationResult", new
                    {
                        isSuccess = false,
                        message = result.Error ?? "ثبت‌نام ناموفق",
                        errors = new[] { new { code = "REGISTER_FAILED", message = result.Error } }
                    });
                }
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("RegistrationResult", new
                {
                    isSuccess = false,
                    message = "خطای سرور در هنگام ثبت‌نام",
                    errors = new[] { new { code = "SERVER_ERROR", message = ex.Message } }
                });
            }
        }

        /// <summary>
        /// بررسی در دسترس بودن نام کاربری — مستقیم از IUserRepository
        /// چون RegisterCommandHandler هم از همین repository استفاده می‌کند
        /// </summary>
        [AllowAnonymous]
        public async Task CheckUsernameAvailability(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName) || userName.Length < 3)
            {
                await Clients.Caller.SendAsync("UsernameCheckResult", new
                {
                    isAvailable = false,
                    userName = userName,
                    message = "نام کاربری باید حداقل ۳ کاراکتر باشد"
                });
                return;
            }

            try
            {
                // همان منطق RegisterCommandHandler — بررسی تکراری بودن نام کاربری
                var existingUser = await _userRepository.GetByUserNameAsync(userName);

                await Clients.Caller.SendAsync("UsernameCheckResult", new
                {
                    isAvailable = existingUser is null,
                    userName = userName
                });
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("UsernameCheckResult", new
                {
                    isAvailable = (bool?)null,
                    userName = userName,
                    errors = new[] { new { code = "SERVER_ERROR", message = ex.Message } }
                });
            }
        }

        // ── Private Helpers ───────────────────────────────────────────────────

        /// <summary>
        /// همان validation که در RegisterCommandHandler وجود دارد
        /// </summary>
        private static bool _isRegisterInputValid(RegisterCommand command) =>
            command is not null &&
            !string.IsNullOrWhiteSpace(command.UserName) &&
            !string.IsNullOrWhiteSpace(command.Password) &&
            !string.IsNullOrWhiteSpace(command.Email) &&
            !string.IsNullOrWhiteSpace(command.FirstName) &&
            !string.IsNullOrWhiteSpace(command.LastName) &&
            command.RoleId != Guid.Empty;

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