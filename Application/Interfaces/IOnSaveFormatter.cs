using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Poplike.Application.Interfaces;

public interface IOnSaveFormatter
{
    void Format(ChangeTracker changeTracker);
}
