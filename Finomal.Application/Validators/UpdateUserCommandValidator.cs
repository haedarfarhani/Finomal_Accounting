using Finomal.Application.Users.UpdateUser;
using FluentValidation;

namespace Finomal.Application.Validators
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .Must(id => Guid.TryParse(id, out _)).WithMessage("شناسه کاربر نامعتبر است.");

            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("نام الزامی است.")
                .MaximumLength(50);

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("نام خانوادگی الزامی است.")
                .MaximumLength(50);

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("فرمت ایمیل نامعتبر است.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^09[0-9]{9}$").WithMessage("شماره موبایل نامعتبر است.")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

            RuleFor(x => x.RoleId)
                .NotEmpty()
                .Must(id => Guid.TryParse(id, out _)).WithMessage("شناسه نقش نامعتبر است.");
        }
    }
}
