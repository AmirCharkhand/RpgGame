namespace RPG.Application.Models.WeaponDtos;

public class AddWeaponDto
{
    public string Name { get; set; } = string.Empty;
    public float Damage { get; set; }
    public int CharacterRef { get; set; }
}