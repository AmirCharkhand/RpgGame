namespace RPG.Application.Models;

public class FilterDto
{
    public string PropertyName { get; set; } = string.Empty;
    public ExpressionOperatorType OperatorType { get; set; }
    public string Value { get; set; } = string.Empty;
}