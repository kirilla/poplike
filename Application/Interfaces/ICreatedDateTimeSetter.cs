using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Poplike.Application.Interfaces
{
    public interface ICreatedDateTimeSetter
    {
        void SetCreated(ChangeTracker changeTracker);
    }
}
