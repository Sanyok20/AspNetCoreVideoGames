using FluentValidation;
using VideoGames.BLL.Dtos.Developer;

namespace VideoGames.BLL.Validators.Developer
{
    public class UpdateDeveloperValidator : AbstractValidator<UpdateDeveloperDto>
    {
        public UpdateDeveloperValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id повинна бути більшою за 0");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Назва розробника не може бути порожньою");
        }
    }
}