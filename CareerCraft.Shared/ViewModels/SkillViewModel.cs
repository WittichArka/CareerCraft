using System.ComponentModel.DataAnnotations;

namespace CareerCraft.Shared.ViewModels;

public class SkillViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Le nom est obligatoire")]
    [Display(Name = "Nom de la compétence")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Le niveau est obligatoire")]
    [Range(1, 5, ErrorMessage = "Le niveau doit être entre 1 et 5")]
    [Display(Name = "Niveau")]
    public int Level { get; set; }

    [Display(Name = "Catégorie")]
    public string? Category { get; set; }
}
