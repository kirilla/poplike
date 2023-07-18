namespace Poplike.Domain;

public class Subject : IEntity, ICreatedDateTime, IUpdatedDateTime
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool MultipleChoice { get; set; }
    public bool FreeExpression { get; set; }

    public int StatementCount { get; set; }

    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public List<Keyword> Keywords { get; set; }
    public List<SubjectContact> SubjectContacts { get; set; }
    public List<SubjectBlurb> SubjectBlurbs { get; set; }
    public List<Statement> Statements { get; set; }

    public EntityKind EntityKind => EntityKind.Subject;
}
