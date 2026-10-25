using FluentValidation;
using VideoGames.BLL.Dtos.User;
using VideoGames.DAL.Repositories;

namespace VideoGames.BLL.Validators.User
{
    public class ChangeEmailValidator : AbstractValidator<ChangeEmailDto>
    {
        public ChangeEmailValidator(UserRepository userRepository)
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Пошта є обов'язковою")
                .Matches(@"^[a-zA-Z0-9](?:[a-zA-Z0-9._%+-]*[a-zA-Z0-9])?@[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]*[a-zA-Z0-9])?)+$").WithMessage("Невірний формат пошти")
                .MustAsync(async (email, ct) => !await userRepository.IsExistsEmailAsync(email, ct))
                .WithMessage(x => $"Пошта '{x.Email}' вже використовуєтся");
        }
    }
}