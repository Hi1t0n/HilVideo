using System.ComponentModel.DataAnnotations;
using UserService.Domain.Models;

namespace AuthService.Domain.Contracts;

public record UserRegisterRequest([Required][MaxLength(30)]string Login, [Required][MaxLength(30)]string Password, string? Email, string? PhoneNumber);