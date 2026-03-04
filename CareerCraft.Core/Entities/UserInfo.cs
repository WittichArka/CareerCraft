namespace CareerCraft.Core.Entities;

public class UserInfo
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int Order { get; set; }
    public string Title { get; set; } = string.Empty; // Phone, Email, ...
    public string? ImagePath { get; set; }
    public string? CustomCssClass { get; set; }
    public string Value { get; set; } = string.Empty;

    public User? User { get; set; }
}
