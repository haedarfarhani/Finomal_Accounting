namespace Finomal.Application.Users.Dtos
{
    public class LoginDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; } // JWT یا هر توکن احراز هویت
        public DateTime TokenExpiry { get; set; } // تاریخ انقضای توکن
        public bool IsSuccess { get; set; }
        public string Message { get; set; } // پیام خطا یا موفقیت
        public Guid RoleId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiry { get; set; }
    }
}
