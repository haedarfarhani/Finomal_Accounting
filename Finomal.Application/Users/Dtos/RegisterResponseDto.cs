using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finomal.Application.Users.Dtos
{
    public class RegisterResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int? UserId { get; set; } 
        public string UserName { get; set; } 
    }
}
