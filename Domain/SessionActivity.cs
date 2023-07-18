namespace Poplike.Domain;

public class SessionActivity : IEntity, ICreatedDateTime
{
    public int Id { get; set; }

    public DateTime? Created { get; set; }

    public int SessionId { get; set; }
    public Session Session { get; set; }

    public EntityKind EntityKind => EntityKind.SessionActivity;
}
