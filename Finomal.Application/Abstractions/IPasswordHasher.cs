using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finomal.Application.Abstractions
{
    /// <summary>
    /// قرارداد هشر رمز عبور.
    /// پیاده‌سازی پیش‌فرض از BCrypt استفاده می‌کند (در Infrastructure).
    /// </summary>
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string hashedPassword);
    }
}
