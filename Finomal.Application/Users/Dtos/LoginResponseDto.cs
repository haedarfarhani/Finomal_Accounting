using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finomal.Application.Users.Dtos
{
    public class LoginResponseDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime TokenExpiry { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
