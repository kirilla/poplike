namespace Poplike.Domain;

public class Expression : IEntity, ICreatedDateTime, IUpdatedDateTime
{
    public int Id { get; set; }

    public string Characters { get; set; }

    public int Order { get; set; }

    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }

    public int ExpressionSetId { get; set; }
    public ExpressionSet ExpressionSet { get; set; }

    public EntityKind EntityKind => EntityKind.Expression;
}
