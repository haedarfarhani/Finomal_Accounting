using Finomal.Application.Users.ChangePassword;
using FluentValidation;

namespace Finomal.Application.Validators
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .Must(id => Guid.TryParse(id, out _)).WithMessage("شناسه کاربر نامعتبر است.");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().WithMessage("رمز عبور فعلی الزامی است.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("رمز عبور جدید الزامی است.")
                .MinimumLength(8).WithMessage("رمز عبور جدید باید حداقل ۸ کاراکتر باشد.")
                .NotEqual(x => x.CurrentPassword).WithMessage("رمز عبور جدید نباید با رمز فعلی یکسان باشد.");
        }
    }
}
