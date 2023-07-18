using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Poplike.Application.Interfaces
{
    public interface IUpdatedDateTimeSetter
    {
        void SetUpdated(ChangeTracker changeTracker);
    }
}
