using RPG.Application.Models.WeaponDtos;

namespace RPG.Application.Models.CharacterDtos;

public class GetUniversalCharacterDto
{
    public int Id { get; set; }
    public bool Owned { get; set; } = false;
    public string Name { get; set; } = string.Empty;
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