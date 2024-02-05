using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.ErrorObjects;

public class BadRequestError : IError
{
    public string ErrorMessange { get; }

    public BadRequestError(string errorMessange)
    {
        ErrorMessange = errorMessange;
    }
}