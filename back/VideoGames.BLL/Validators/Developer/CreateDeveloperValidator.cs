using FluentValidation;
using VideoGames.BLL.Dtos.Developer;

namespace VideoGames.BLL.Validators.Developer
{
    public class CreateDeveloperValidator : AbstractValidator<CreateDeveloperDto>
    {
        public CreateDeveloperValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Назва розробника не може бути порожньою");
        }
    }
}