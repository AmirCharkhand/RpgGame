using System.Text.Json.Serialization;

namespace RPG.Application.Models;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ExpressionOperatorType
{
    Equal,
    GraterThan,
    LessThan
}