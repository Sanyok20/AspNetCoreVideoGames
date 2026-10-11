using FluentValidation;
using VideoGames.BLL.Dtos.Auth;

namespace VideoGames.BLL.Validators.Auth
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty().WithMessage("Поле логін не може бути порожнім");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Поле пароль не може бути порожнім")
                .MinimumLength(6).WithMessage("Мінімальна довжина паролю 6 символів");
        }
    }
}