using System.ComponentModel.DataAnnotations;

namespace CareerCraft.Web.Models;

public class UserInfoViewModel
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int Order { get; set; }

    [Required(ErrorMessage = "Le titre est obligatoire")]
    [Display(Name = "Titre (Email, Mobile, ...)")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Icône (SVG ou image)")]
    public string? ImagePath { get; set; }

    [Display(Name = "Classe CSS personnalisée")]
    public string? CustomCssClass { get; set; }

    [Required(ErrorMessage = "La valeur est obligatoire")]
    [Display(Name = "Valeur")]
    public string Value { get; set; } = string.Empty;
}
