using Finomal.Domain.Abstractions;
using Finomal.Application.Users.Dtos;

namespace Finomal.Application.Users
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// هش کردن رمز عبور با استفاده از الگوریتم امن (BCrypt یا مشابه)
        /// </summary>
        string GeneratePasswordHash(string password);

        /// <summary>
        /// بررسی صحت رمز عبور ورودی با هش ذخیره‌شده
        /// </summary>
        bool VerifyPassword(string password, string passwordHash);

        /// <summary>
        /// ثبت‌نام کاربر جدید
        /// </summary>
        /// <returns>در صورت موفقیت → LoginDto (شامل توکن و اطلاعات کاربر)</returns>
        Task<Result<LoginDto>> RegisterUserAsync(
            string username,
            string email,
            string password,
            string? firstName = null,
            string? lastName = null);

        /// <summary>
        /// ورود کاربر با نام کاربری، رمز عبور و نقش انتخابی
        /// </summary>
        /// <returns>در صورت موفقیت → LoginDto | در صورت شکست → پیام خطا</returns>
        Task<Result<LoginDto>> LoginUserAsync(string username, string password, Guid roleId);

        /// <summary>
        /// خروج کاربر (معمولاً invalidate کردن refresh token یا session)
        /// </summary>
        /// <param name="userId">شناسه کاربر فعلی (معمولاً از Claims گرفته می‌شود)</param>
        Task<Result> LogoutUserAsync(Guid userId);

        /// <summary>
        /// تغییر رمز عبور توسط خود کاربر (نیاز به دانستن رمز فعلی)
        /// </summary>
        Task<Result> ChangePasswordAsync(
            Guid userId,
            string currentPassword,
            string newPassword);

        /// <summary>
        /// بازنشانی رمز عبور (معمولاً بعد از ارسال لینک ریست)
        /// </summary>
        Task<Result> ResetPasswordAsync(
            Guid userId,
            string newPassword,
            string? resetToken = null);   // اگر از توکن ریست استفاده می‌کنی
    }
}