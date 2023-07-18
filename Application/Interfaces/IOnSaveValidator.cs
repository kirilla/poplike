using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Poplike.Application.Interfaces;

public interface IOnSaveValidator
{
    void Validate(ChangeTracker changeTracker);
}
