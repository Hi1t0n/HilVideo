namespace AuthService.Domain.Interfaces;

public interface ICheckUserData
{
    Task<bool> CheckUserLogin(string login);
    Task<bool> CheckUserEmail(string email);
    Task<bool> CheckUserPhoneNumber(string phoneNumber);
}