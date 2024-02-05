using UserService.Domain.Interfaces;

namespace UserService.Infrastructure.ErrorObjects;

public class NotFoundError : IError
{
    public string ErrorMessange { get; }

    public NotFoundError(string errorMessange)
    {
        ErrorMessange = errorMessange;
    }
}