using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.ErrorObjects;

public class UnauthorizedError : IError
{
    public string ErrorMessange { get; }

    public UnauthorizedError(string errorMessange)
    {
        ErrorMessange = errorMessange;
    }
}