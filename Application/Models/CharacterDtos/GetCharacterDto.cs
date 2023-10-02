using RPG.Application.Models.WeaponDtos;

namespace RPG.Application.Models.CharacterDtos;

public class GetCharacterDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "Amiroo";
    public float Health { get; set; }
    public float Strength { get; set; }
    public float Attack { get; set; }
    public float Defence { get; set; }
    public CharacterType Type { get; set; }
    public int Fights { get; set; }
    public int Victories { get; set; }
    public int Defeats { get; set; }
    public GetWeaponDto? Weapon { get; set; }
}