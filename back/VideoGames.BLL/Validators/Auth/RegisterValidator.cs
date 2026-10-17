using FluentValidation;
using VideoGames.BLL.Dtos.Auth;
using VideoGames.DAL.Repositories;

namespace VideoGames.BLL.Validators.Auth
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator(UserRepository userRepository)
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Пошта є обов'язковою")
                .Matches(@"^[a-zA-Z0-9](?:[a-zA-Z0-9._%+-]*[a-zA-Z0-9])?@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)+$").WithMessage("Невірний формат пошти")
                .MustAsync(async (email, ct) => !await userRepository.IsExistsEmailAsync(email, ct))
                .WithMessage(x => $"Пошта '{x.Email}' вже використовуєтся");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Ім'я користувача є обов'язковим")
                .MustAsync(async (userName, ct) => !await userRepository.IsExistsUserNameAsync(userName, ct))
                .WithMessage(x => $"Ім'я користувача '{x.UserName}' вже використовуєтся");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль є обов'язковим")
                .MinimumLength(6).WithMessage("Мінімальна довжина паролю 6 символів");
        }
    }
}