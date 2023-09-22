using System.ComponentModel.DataAnnotations;

namespace RPG.Application.Models.WeaponDtos;

public class AddWeaponDto
{
    [Required]
    [StringLength(55)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Range(1,100)]
    public float Damage { get; set; }
    
    [Required]
    public int CharacterRef { get; set; }
}