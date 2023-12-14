using PlaninngResolver.Domain.Application.Rules;
using PlaninngResolver.Domain.Entities;
using PlaninngResolver.Domain.Interfaces;

namespace PlaninngResolver.Domain.Application.TimeTable;

public class TimetableSearch
{
    private readonly ITimetableService _timetableService = new TimetableService();
    private readonly SimulatedAnnealingAcceptor _acceptor ;
    private List<Lecture> _currentTimetable;
    private readonly Random _random;
    private readonly List<ClassRoom> _rooms;
    private double _bestScore = double.MinValue;
    private List<Lecture> _bestTimetable;
    private readonly TimetableScorer _scorer;
    public TimetableSearch(List<Lecture> initialTimetable, List<ClassRoom> rooms)
    {
        _currentTimetable = initialTimetable;
        _rooms = rooms;
        _random = new Random();
        _scorer = new TimetableScorer(_timetableService);
        _acceptor = new SimulatedAnnealingAcceptor( _scorer);
        _acceptor.SetStartingTemperature(100000); // Example starting temperature
    }
    public List<Lecture> RunHillClimbing()
    {
        List<Lecture> currentTimetable = DeepCopyListOfLectures(_currentTimetable);
        var currentScore = _scorer.CalculateScore(currentTimetable);

        int improvementFound = 10000;
        do
        {
            
            List<Lecture> neighbor = GenerateNewTimetable(currentTimetable);
            var neighborScore = _scorer.CalculateScore(neighbor);

            if (neighborScore > currentScore)
            {
                Console.WriteLine("scores of HC: " + neighborScore + ":" + currentScore);
                currentTimetable = DeepCopyListOfLectures(neighbor);
                currentScore = neighborScore;
                
            }
            improvementFound--;
            Console.WriteLine(improvementFound +" "+ currentScore);
        }
        while (improvementFound>0);

        return currentTimetable;
    }

    public void Run(List<Lecture> initialSolution)
    {
        _currentTimetable = new List<Lecture>();
        _currentTimetable = DeepCopyListOfLectures(initialSolution);
        for (double timeGradient = 0; timeGradient <= 1; timeGradient += 0.000001)
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
        var newSolution = DeepCopyListOfLectures(solution);
        int numberOfMutations = _random.Next(1, Math.Max(2, solution.Count / 10));  // Limit the number of mutations

        for (int i = 0; i < numberOfMutations; i++)
        {
            int index = _random.Next(solution.Count);
            Lecture lectureToMutate = newSolution[index];
            /*var shouldMove = ShouldBeMoved(lectureToMutate);
            if(!shouldMove) continue;*/
            int changeType = _random.Next(3);  // Consider adding more mutation types

            switch (changeType)
            {
                case 0: // Change Seance
                    // Ensure the new seance respects the constraints
                    lectureToMutate.Seance = FindNewValidSeance(lectureToMutate);
                    break;
                case 1: // Change Classroom
                    // Change classroom considering capacity and suitability
                    lectureToMutate.ClassRoomId = FindNewValidClassroom(lectureToMutate);
                    lectureToMutate.ClassRoom = _rooms.FirstOrDefault(x => x.Id == lectureToMutate.ClassRoomId);
                    break;
                case 2: // Swap Courses
                    // Ensure the swap makes sense (e.g., teachers are qualified for the new courses)
                    SwapCourses(newSolution, index);
                    break;
                // Additional cases for other types of mutations
            }
        }
        return newSolution;
    }
    private bool ShouldBeMoved(Lecture lecture)
    {
        // Check if the lecture conflicts with any other lecture
        return _currentTimetable.Any(otherLecture => 
            otherLecture.Seance == lecture.Seance && (
                otherLecture.TeacherId == lecture.TeacherId ||
                otherLecture.ClassRoomId == lecture.ClassRoomId ||
                (otherLecture.SectionId == lecture.SectionId && (lecture.GroupeId == null || otherLecture.GroupeId == null)) ||
                (lecture.GroupeId != null && otherLecture.GroupeId == lecture.GroupeId)
            )
        );
    }
// Implement these methods to ensure mutations are valid and meaningful
    private int FindNewValidSeance(Lecture lecture)
    {
        List<int> validSeances = Enumerable.Range(1, 36).ToList(); // Assuming 36 time slots
        // Remove seances that would cause a conflict
        foreach (var l in _currentTimetable)
        {
            if (l.TeacherId == lecture.TeacherId || l.ClassRoomId == lecture.ClassRoomId)
            {
                validSeances.Remove(l.Seance);
            }
        }

        return validSeances.Any() ? validSeances[_random.Next(validSeances.Count)] : lecture.Seance;
    }
    private int FindNewValidClassroom(Lecture lecture)
    {
        var suitableClassrooms = _rooms.Where(r =>r.ClassRoomTypeId == lecture.ClassRoomTypeId).ToList();
        // Filter out classrooms that are already occupied in the desired seance
        suitableClassrooms = suitableClassrooms.Where(r => !_currentTimetable.Any(l => l.ClassRoomId == r.Id && l.Seance == lecture.Seance)).ToList();
        // Return a random suitable and available classroom
        return suitableClassrooms.Any() ? suitableClassrooms[_random.Next(suitableClassrooms.Count)].Id : lecture.ClassRoomId.Value;
    }
    
    private void SwapCourses(List<Lecture> solution, int index)
    {
        var firstLecture = solution[index];
        int secondIndex;
        do
        {
            secondIndex = _random.Next(solution.Count);
        } while (secondIndex == index);

        var secondLecture = solution[secondIndex];

        // Swap CourseId and related attributes
        SwapLectureAttributes(firstLecture, secondLecture);
    }

    private void SwapLectureAttributes(Lecture firstLecture, Lecture secondLecture)
    {
        (firstLecture.CourseId, secondLecture.CourseId) = (secondLecture.CourseId, firstLecture.CourseId);
        (firstLecture.TeacherId, secondLecture.TeacherId) = (secondLecture.TeacherId, firstLecture.TeacherId);
        (firstLecture.SectionId, secondLecture.SectionId) = (secondLecture.SectionId, firstLecture.SectionId);
        (firstLecture.GroupeId, secondLecture.GroupeId) = (secondLecture.GroupeId, firstLecture.GroupeId);
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
                Time = oldLecture.Time, 
                ClassRoom = oldLecture.ClassRoom,
                Teacher = oldLecture.Teacher
                // Repeat for other reference types
            };

            newList.Add(newLecture);
        }

        return newList;
    }
}
