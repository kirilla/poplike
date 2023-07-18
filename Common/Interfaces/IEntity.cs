namespace Poplike.Common.Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }

        EntityKind EntityKind { get; }
    }
}
