﻿namespace Poplike.Common.Dates;

public class DateService : IDateService
{
    public DateTime GetDateTimeNow()
    {
        return DateTime.Now;
    }
}
