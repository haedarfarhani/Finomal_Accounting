using Finomal.Application.Users.CreateUser;
using FluentValidation;

namespace Finomal.Application.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("نام الزامی است.")
                .MaximumLength(50).WithMessage("نام نباید بیشتر از ۵۰ کاراکتر باشد.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("نام خانوادگی الزامی است.")
                .MaximumLength(50).WithMessage("نام خانوادگی نباید بیشتر از ۵۰ کاراکتر باشد.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("نام کاربری الزامی است.")
                .MinimumLength(3).WithMessage("نام کاربری باید حداقل ۳ کاراکتر داشته باشد.")
                .MaximumLength(50).WithMessage("نام کاربری نباید بیشتر از ۵۰ کاراکتر باشد.")
                .Matches(@"^[a-zA-Z0-9._\-]+$").WithMessage("نام کاربری فقط می‌تواند حروف انگلیسی، عدد، نقطه، خط‌تیره و زیرخط داشته باشد.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("فرمت ایمیل نامعتبر است.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^09[0-9]{9}$").WithMessage("شماره موبایل باید با ۰۹ شروع شده و ۱۱ رقم باشد.")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("رمز عبور الزامی است.")
                .MinimumLength(8).WithMessage("رمز عبور باید حداقل ۸ کاراکتر داشته باشد.")
                .Matches(@"[A-Z]").WithMessage("رمز عبور باید حداقل یک حرف بزرگ داشته باشد.")
                .Matches(@"[0-9]").WithMessage("رمز عبور باید حداقل یک عدد داشته باشد.");

            RuleFor(x => x.RoleId)
                .NotEmpty().WithMessage("انتخاب نقش الزامی است.")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("شناسه نقش نامعتبر است.");
        }
    }
}
