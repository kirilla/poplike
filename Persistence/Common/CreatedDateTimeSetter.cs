using Poplike.Common.Dates;
using Poplike.Common.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Poplike.Persistence.Common;

public class CreatedDateTimeSetter : ICreatedDateTimeSetter
{
    private readonly IDateService _dateService;

    public CreatedDateTimeSetter(IDateService dateService)
    {
        _dateService = dateService;
    }

    public void SetCreated(ChangeTracker changeTracker)
    {
        var entries = changeTracker
            .Entries()
            .Where(x => x.State == EntityState.Added)
            .Select(x => x.Entity as ICreatedDateTime)
            .Where(x =>
                x != null &&
                x.Created == null)
            .ToList();

        foreach (var entry in entries)
        {
            entry!.Created = _dateService.GetDateTimeNow();
        }
    }
}
