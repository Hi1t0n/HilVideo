using AuthService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.Context;

namespace AuthService.Infrastructure.Helpers;

public class CheckUserData : ICheckUserData
{
    private readonly ApplicationDbContext _context;

    public CheckUserData(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CheckUserLogin(string login)
    {
        var result = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        return result != null;
    }

    public async Task<bool> CheckUserEmail(string email)
    {
        var result = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        return result != null;
    }

    public async Task<bool> CheckUserPhoneNumber(string phoneNumber)
    {
        var result = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        return result != null;
    }
}

