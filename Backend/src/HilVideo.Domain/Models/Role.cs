namespace UserService.Domain.Models;

public class Role
{
    public Guid RoleId { get; set; }
    
    public string RoleName { get; set; }

    public List<User> Users { get; set; } = new();
}