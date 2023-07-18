namespace Poplike.Domain;

public class Statement : IEntity, ICreatedDateTime
{
    public int Id { get; set; }

    public string Sentence { get; set; }

    public bool UserCreated { get; set; }

    public int Order { get; set; }

    public DateTime? Created { get; set; }

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }

    public List<UserStatement> UserStatements { get; set; }

    public EntityKind EntityKind => EntityKind.Statement;
}
