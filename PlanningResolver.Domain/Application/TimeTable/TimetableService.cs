using PlaninngResolver.Domain.Interfaces;

namespace PlaninngResolver.Domain.Application.TimeTable;

public class TimetableService : ITimetableService
{
    private readonly int _daysInWeek;
    private readonly int _slotsPerDay;

    public TimetableService(int daysInWeek = 6, int slotsPerDay = 6)
    {
        if (daysInWeek > 7)
        {
            throw new InvalidOperationException("days in week cannot be upper then 7.");
        }
        _daysInWeek = daysInWeek;
        _slotsPerDay = slotsPerDay;
    }

    public List<int> GetFirstHalfPeriodsPerWeek()
    {
        return GetHalfPeriodsPerWeek(firstHalf: true);
    }

    public List<int> GetSecondHalfPeriodsPerWeek()
    {
        return GetHalfPeriodsPerWeek(firstHalf: false);
    }

    private List<int> GetHalfPeriodsPerWeek(bool firstHalf)
    {
        var periods = new List<int>();
        int startSlot = firstHalf ? 1 : _slotsPerDay / 2 + 1;
        int endSlot = firstHalf ? _slotsPerDay / 2 : _slotsPerDay;

        for (int day = 1; day <= _daysInWeek; day++)
        {
            for (int slot = startSlot; slot <= endSlot; slot++)
            {
                int periodNumber = (day - 1) * _slotsPerDay + slot;
                periods.Add(periodNumber);
            }
        }

        return periods;
    }
}
