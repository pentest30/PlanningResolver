using PlaninngResolver.Domain.Application.TimeTable;
using PlaninngResolver.Domain.Entities;

namespace PlaninngResolver.Domain.Application.Rules;

public class SimulatedAnnealingAcceptor
{
    private double[] _temperatureLevels;
    public readonly int _levelsLength = 1; // Simple example with one temperature level
    private const double TemperatureMinimum = 1.0E-100;
    private readonly TimetableScorer _scorer;
    public SimulatedAnnealingAcceptor(TimetableScorer scorer)
    {
        _scorer = scorer;
    }
    public void SetStartingTemperature(double startingTemperature)
    {
        _temperatureLevels = new[] { startingTemperature };
    }

    public bool IsAccepted(List<Lecture> currentTimetable, List<Lecture> newTimetable)
    {
        double currentScore = _scorer.CalculateScore(currentTimetable);
        double newScore = _scorer.CalculateScore(newTimetable);
        Console.WriteLine(currentScore + " " + newScore);
        if (newScore > currentScore)
        {
            return true;
        }
        double scoreDifference = currentScore - newScore;
        double acceptChance = Math.Exp(-scoreDifference / _temperatureLevels[0]);

        return new Random().NextDouble() < acceptChance;
    }

    public void UpdateTemperature(double timeGradient)
    {
        double reverseTimeGradient = 1.0 - timeGradient;
        _temperatureLevels[0] *= reverseTimeGradient;
        if (_temperatureLevels[0] < TemperatureMinimum)
        {
            _temperatureLevels[0] = TemperatureMinimum;
        }
    }
}
