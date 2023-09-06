using System.Text.Json.Serialization;

namespace RPG.Application.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CharacterType
{
    Celtic,
    Knight,
    Mage,
    Healer
}