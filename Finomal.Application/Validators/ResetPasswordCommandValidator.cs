using Finomal.Application.Users.ResetPassword;
using FluentValidation;

namespace Finomal.Application.Validators
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .Must(id => Guid.TryParse(id, out _)).WithMessage("شناسه کاربر نامعتبر است.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("رمز جدید الزامی است.")
                .MinimumLength(8).WithMessage("رمز عبور باید حداقل ۸ کاراکتر باشد.");
        }
    }
}
