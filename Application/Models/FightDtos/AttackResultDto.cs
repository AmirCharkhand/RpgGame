namespace RPG.Application.Models.FightDtos;

public class AttackResultDto
{
    public string AttackerName { get; set; } = string.Empty;
    public string OpponentName { get; set; } = string.Empty;
    public float OpponentHp { get; set; }
}