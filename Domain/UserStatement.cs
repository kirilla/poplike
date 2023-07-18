namespace Poplike.Domain;

public class UserStatement : IEntity, ICreatedDateTime, IUpdatedDateTime
{
    public int Id { get; set; }

    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public int StatementId { get; set; }
    public Statement Statement { get; set; }

    public EntityKind EntityKind => EntityKind.UserStatement;
}
