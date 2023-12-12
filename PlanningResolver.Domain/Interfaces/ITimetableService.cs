namespace PlaninngResolver.Domain.Interfaces;

public interface ITimetableService
{
    List<int> GetFirstHalfPeriodsPerWeek();
    List<int> GetSecondHalfPeriodsPerWeek();
}