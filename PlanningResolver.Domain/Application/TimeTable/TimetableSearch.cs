using PlaninngResolver.Domain.Entities;

namespace PlaninngResolver.Domain.Application.Rules;

public class TimetableSearch
{
    private readonly ITimetableService _timetableService = new TimetableService();
    private readonly SimulatedAnnealingAcceptor _acceptor ;
    private List<Lecture> _currentTimetable;
    private readonly Random _random;
    private readonly List<ClassRoom> _rooms;
    private double _bestScore = double.MinValue;
    private List<Lecture> _bestTimetable;
    private TimetableScorer _scorer;
    public TimetableSearch(List<Lecture> initialTimetable, List<ClassRoom> rooms)
    {
        _currentTimetable = initialTimetable;
        _rooms = rooms;
        _random = new Random();
        _scorer = new TimetableScorer(_timetableService);
        _acceptor = new SimulatedAnnealingAcceptor( _scorer);
        _acceptor.SetStartingTemperature(100000); // Example starting temperature
    }

    public void Run()
    {
        for (double timeGradient = 0; timeGradient <= 1; timeGradient += 0.00001)
        {
            List<Lecture> newTimetable = GenerateNewTimetable(_currentTimetable); // Method to generate new timetable
            var newScore = _scorer.CalculateScore(newTimetable);
            if (_acceptor.IsAccepted(_currentTimetable, newTimetable))
            {
                if (newScore > _bestScore)
                {
                    _bestTimetable = new List<Lecture>(newTimetable);
                    _bestScore = newScore;
                    Console.WriteLine("Best score: " + _bestScore);
                   // _currentTimetable = new List<Lecture>(_bestTimetable);
                } 
                _currentTimetable = new List<Lecture>(newTimetable);
            }
            
            _acceptor.UpdateTemperature(timeGradient);
        }
        
    }
    public double GetBestScore()
    {
        return _bestScore;
    }

    public List<Lecture> GetBestTimetable()
    {
        return _bestTimetable;
    }

    private List<Lecture> GenerateNewTimetable(List<Lecture> solution)
    {
        // Implement logic to generate a new timetable
        var newSolution = DeepCopyListOfLectures(solution);
        int numberOfMutations = _random.Next(solution.Count/100);

        for (int i = 0; i < numberOfMutations; i++)
        {
            int index = _random.Next(solution.Count);
            int changeType = _random.Next(3); // Increased range for additional mutation types

            switch (changeType)
            {
                case 0: // Change Seance
                    newSolution[index].Seance = _random.Next(1, 36);
                    break;
                case 1: // Change Classroom
                    var rooms = _rooms.Where(x => x.ClassRoomTypeId == newSolution[index].ClassRoomTypeId).ToList();
                    newSolution[index].ClassRoomId = rooms[_random.Next(rooms.Count)].Id;
                    break;
                case 2:
                {
                    int firstIndex = _random.Next(solution.Count);
                    int secondIndex;
                    do
                    {
                        secondIndex = _random.Next(solution.Count);

                    } while (secondIndex == firstIndex);

                    (newSolution[firstIndex].CourseId, newSolution[secondIndex].CourseId) = (newSolution[secondIndex].CourseId, newSolution[firstIndex].CourseId);
                }
                    break;
                // Add more cases as needed
            }
        }

        return newSolution.ToList();
    }

    private List<Lecture> DeepCopyListOfLectures(List<Lecture> originalList)
    {
        var newList = new List<Lecture>();

        foreach (var oldLecture in originalList)
        {
            var newLecture = new Lecture
            {
                Id = oldLecture.Id,
                TeacherId = oldLecture.TeacherId,
                ClassRoomTypeId = oldLecture.ClassRoomTypeId,
                Periode = oldLecture.Periode,
                ClassRoomId = oldLecture.ClassRoomId,
                CourseId = oldLecture.CourseId,
                Seance = oldLecture.Seance,
                AnneeId = oldLecture.AnneeId,
                SectionId = oldLecture.SectionId,
                GroupeId = oldLecture.GroupeId,
                FaculteId = oldLecture.FaculteId,
                DepartementId = oldLecture.DepartementId,
                SpecialiteId = oldLecture.SpecialiteId,
                Solved = oldLecture.Solved,
                Display = oldLecture.Display,
                Jour = oldLecture.Jour,
                Time = oldLecture.Time
                // Repeat for other reference types
            };

            newList.Add(newLecture);
        }

        return newList;
    }
}
