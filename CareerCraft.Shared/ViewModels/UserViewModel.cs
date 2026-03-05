using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CareerCraft.Shared.ViewModels;

public class UserViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le prénom est obligatoire")]
    [Display(Name = "Prénom")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le nom est obligatoire")]
    [Display(Name = "Nom")]
    public string LastName { get; set; } = string.Empty;

    [Display(Name = "Genre")]
    public string? Gender { get; set; }

    [Display(Name = "Photo de profil")]
    public string? PortraitPath { get; set; }

    public List<UserInfoViewModel> UserInfos { get; set; } = new List<UserInfoViewModel>();
}
