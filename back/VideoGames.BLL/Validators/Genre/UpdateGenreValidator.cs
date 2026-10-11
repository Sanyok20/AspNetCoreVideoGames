using FluentValidation;
using VideoGames.BLL.Dtos.Genre;
using VideoGames.DAL.Repositories;

namespace VideoGames.BLL.Validators.Genre
{
    public class UpdateGenreValidator : AbstractValidator<UpdateGenreDto>
    {
        public UpdateGenreValidator(GenreRepository genreRepository)
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id жанру повинен бути більший за 0");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Назва жанру не може бути порожньою")
                .MaximumLength(100).WithMessage("Назва жанру не може перевищувати 100 символів")
                .MustAsync(async (x, ct) => !await genreRepository.IsExistsAsync(x, ct))
                .WithMessage(x => $"Жанр з назвою '{x.Name}' вже існує");
        }
    }
}