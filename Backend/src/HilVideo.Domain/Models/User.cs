namespace UserService.Domain.Models;

public class User
{

    public Guid UserId { get; set; }

    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public Guid RoleId { get; set; }
    
    public Role Role { get; set; }

    public string? Email { get; set; } = string.Empty;
    
    public string? PhoneNumber { get; set; } = string.Empty;
    
    public string CreatedDate { get; set; }
    public List<Movie> Movies { get; set; } = new();
    public List<FavoriteMoviesUsers> FavoriteMoviesUsers { get; set; } = new();
}