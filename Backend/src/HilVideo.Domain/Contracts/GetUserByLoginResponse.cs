namespace UserService.Domain.Contracts;

public record GetUserByLoginResponse(Guid Id, string Login, string RoleName, string Email,string PhoneNumber,string CreatedDate);
