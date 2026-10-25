using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using VideoGames.BLL.Dtos;

namespace VideoGames.Extensions
{
    public static class ControllerExtensions
    {
        public static IActionResult GetHttpResponse(this ControllerBase controller, ResponseDto response)
        {
            if (response.IsSuccess)
            {
                return controller.Ok(response);
            }

            return controller.BadRequest(response);
        }

        public static IActionResult ValidationResponse(this ControllerBase controller, ValidationResult validationResult)
        {
            var errors = new Dictionary<string, string>();

            foreach (var error in validationResult.Errors)
            {
                errors.TryAdd(error.PropertyName, error.ErrorMessage);
            }

            var responseDto = ResponseDto.Error("Помилка валідації", errors);

            return controller.BadRequest(responseDto);
        }

        public static string? GetJwtToken(this ControllerBase controller)
        {
            var context = controller.HttpContext;
            var user = controller.User;

            var authorization = context.Request.Headers.Authorization.First();

            if (user.Identity == null || !user.Identity.IsAuthenticated || authorization == null)
            {
                return null;
            }

            string token = new string(authorization.Skip(7).ToArray());

            return token;
        }
    }
}