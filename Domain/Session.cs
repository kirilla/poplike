namespace Poplike.Domain;

public class Session : IEntity, ICreatedDateTime, IUpdatedDateTime
{
    public int Id { get; set; }

    public Guid? Guid { get; set; }

    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public List<SessionActivity> SessionActivities { get; set; }

    public EntityKind EntityKind => EntityKind.Session;
}
