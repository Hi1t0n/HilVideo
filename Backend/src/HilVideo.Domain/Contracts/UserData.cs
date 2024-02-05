namespace AuthService.Domain.Contracts;

public record UserData(Guid UserId,
    string Login,
    string RoleName,
    string? PhoneNumber,  
    string? Email,
    string CreatedDate
    );