namespace Poplike.Domain;

public class ExpressionSet : IEntity
{
    public int Id { get; set; }

    public string Emoji { get; set; }
    public string Name { get; set; }

    public bool MultipleChoice { get; set; }
    public bool FreeExpression { get; set; }

    public List<Expression> Expressions { get; set; }
    public List<Category> Categories { get; set; }

    public EntityKind EntityKind => EntityKind.ExpressionSet;
}
