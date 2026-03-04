namespace CareerCraft.Core.Entities;

using System.Collections.Generic;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Gender { get; set; }
    public string? PortraitPath { get; set; }

    public ICollection<UserInfo> UserInfos { get; set; } = new List<UserInfo>();
}
