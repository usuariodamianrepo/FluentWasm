namespace Backend.Application.Validations
{
    using Backend.Application.DTOs;

    using FluentValidation;

    public interface IValidationService
    {
        Task<ServiceResponse> ValidateAsync<T>(T model, IValidator<T> validator);
    }
}
