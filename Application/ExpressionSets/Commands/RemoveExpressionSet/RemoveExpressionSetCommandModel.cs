namespace Poplike.Application.ExpressionSets.Commands.RemoveExpressionSet;

public class RemoveExpressionSetCommandModel
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool Confirmed { get; set; }
}
