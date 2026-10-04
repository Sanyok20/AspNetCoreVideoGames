using FluentValidation;
using VideoGames.BLL.Dtos.Game;
using VideoGames.DAL.Repositories;

namespace VideoGames.BLL.Validators.Game
{
    public class UpdateGameValidator : AbstractValidator<UpdateGameDto>
    {
        public UpdateGameValidator(DeveloperRepository developerRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id повинна бути більшою за 0");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва гри не може бути порожньою");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Ціна повинна бути більша або дорівнювати 0")
                .LessThanOrEqualTo(decimal.MaxValue).WithMessage($"Ціна повинна бути менша або дорівнювати {decimal.MaxValue}");

            RuleFor(x => x.Rating)
                .GreaterThanOrEqualTo(0).WithMessage("Рейтинг повинен бути від 0 до 10")
                .LessThanOrEqualTo(10).WithMessage("Рейтинг повинен бути від 0 до 10");

            RuleFor(x => x.DeveloperId)
                .MustAsync(async (id, ct) =>
                {
                    return await developerRepository.IsExistAsync(id, ct);
                }).WithMessage(dto => $"Розробник з id '{dto.DeveloperId}' не існує");
        }
    }
}