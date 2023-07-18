namespace Poplike.Domain;

public class PasswordResetRequest : IEntity, ICreatedDateTime
{
    public int Id { get; set; }

    public Guid? Guid { get; set; }

    public DateTime? Created { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }

    public EntityKind EntityKind => EntityKind.PasswordResetRequest;
}
