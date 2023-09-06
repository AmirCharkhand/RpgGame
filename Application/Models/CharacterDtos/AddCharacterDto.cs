using System.ComponentModel.DataAnnotations;

namespace RPG.Application.Models.CharacterDtos;

public class AddCharacterDto
{
    [Required]
    [StringLength(55,MinimumLength = 3)]
    public string Name { get; set; } = "Amiroo";
    
    [Required]
    [Range(0,200)]
    public float Health { get; set; }
    
    [Required]
    [Range(0,200)]
    public float Strength { get; set; }
    
    [Required]
    [Range(0,200)]
    public float Attack { get; set; }
    
    [Required]
    [Range(0,200)]
    public float Defence { get; set; }
    
    public CharacterType Type { get; set; }
}