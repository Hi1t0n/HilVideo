namespace UserService.Domain.DTO.AuthDTO;

public record LoginDataResponse(Guid Id,string Login,string Role, string? Email, string? PhoneNumber, string CreateDate, string Token);