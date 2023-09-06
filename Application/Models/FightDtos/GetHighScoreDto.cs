﻿namespace RPG.Application.Models.FightDtos;

public class GetHighScoreDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Fights { get; set; }
    public int Victories { get; set; }
    public int Defeats { get; set; }
}