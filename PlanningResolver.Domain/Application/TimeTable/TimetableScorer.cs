using PlaninngResolver.Domain.Entities;

namespace PlaninngResolver.Domain.Application.Rules;

public class TimetableScorer
{
    private readonly ITimetableService _timetableService;

    public TimetableScorer(ITimetableService timetableService)
    {
        _timetableService = timetableService;
    }
    public double CalculateScore(List<Lecture> generation)
    {
        double score = 0;
        score-= CalculateGroupConflicts(generation);
        score-= CalculateSectionConflicts(generation);
        score-= CalculateTeacherConflicts(generation);
        score-=CalculateClassRoomConflicts(generation);
        score *= 1000;
        score += CalculateSoftFitness(generation);
        // Add more scoring rules based on other constraints

        return  score;
    }

    private static double CalculateGroupConflicts(List<Lecture> lectures)
    {
        var score = 0;
        foreach (var group in lectures.Where(x => x.GroupeId != null).GroupBy(x => x.GroupeId))
        {
            for (int i = 1; i <= 36; i++)
            {
                if (group.Count(x => x.Seance == i) > 1)
                {
                    score += 1;

                }
            }
        }

        return score;
    }

    private static double CalculateSectionConflicts(List<Lecture> generation)
    {
        var score = 0;
        foreach (var section in generation.GroupBy(x => x.SectionId))
        {
            for (int i = 1; i <= 36; i++)
            {
                var totalInSection = section.Count(x => x.Seance == i);
                var totalInGroup = section.Count(w => w.GroupeId != 0 && w.Seance == i);

                if (totalInGroup > 0 && totalInSection > totalInGroup || totalInGroup == 0 && totalInSection > 1)
                {
                    score += 1;
                   
                }
            }
        }
        return score;
    }

    private static double CalculateTeacherConflicts(List<Lecture> generation)
    {
        var score = 0;
        foreach (var teacher in generation.GroupBy(x => x.TeacherId))
        {
            for (int i = 0; i < 36; i++)
            {
                if (teacher.Count(w => w.Seance == i) > 1)
                {
                    score ++;
                    
                }
            }
        }
        return score;
    }

    private static double CalculateClassRoomConflicts(List<Lecture> generation)
    {
        var score = 0;
        foreach (var room in generation.GroupBy(x => x.ClassRoomId))
        {
            for (int i = 1; i <= 36; i++)
            {
                if (room.Count(w => w.Seance == i) > 1)
                {
                    
                    score++;
                   
                }
            }
        } 
        return score;
        
    }

    private int CalculateSoftFitness(List<Lecture> solution)
    {
        int score = 0;

        // Penalty/Bonus based on seance timing
        foreach (var lecture in solution)
        {
            if (CheckIfExistInFirstPeriod(lecture.Seance) && lecture.Periode != 2)
                score += 10;
            else if (CheckIfExistForSecondPeriod(lecture.Seance) && lecture.Periode != 2)
                score -= 10;

            if (CheckIfExistInFirstPeriod(lecture.Seance) && lecture.Periode == 1)
                score += 10;
            else if (CheckIfExistForSecondPeriod(lecture.Seance) && lecture.Periode == 2)
                score += 10;
            else if (CheckIfExistInFirstPeriod(lecture.Seance) && lecture.Periode == 2)
                score -= 10;
            else if (CheckIfExistForSecondPeriod(lecture.Seance) && lecture.Periode == 1)
                score -= 10;
        }
        

        // Teacher Availability Check
        foreach (var group in solution.GroupBy(x => x.TeacherId))
        {
            foreach (var lecture in group)
            {
                if (lecture.Teacher?.Seances.Count > 0)
                {
                    score += lecture.Teacher.Seances.Any(x => x.Number == lecture.Seance)
                        ? -10
                        : 10;
                }
            }
        }

        // Room Availability and Succession Check
        foreach (var group in solution.GroupBy(x => x.ClassRoomId))
        {
            var lectures = group.OrderBy(l => l.Seance).ToList();

            /*foreach (var lecture in lectures)
            {
                if (lecture.ClassRoom.SeanceLbrSalles.Count > 0)
                {
                    score += lecture.ClassRoom.SeanceLbrSalles.Any(x => x.Number == lecture.Seance)
                        ? -100
                        : 100;
                }
            }*/
            // Succession Penalty/Bonus
            for (int i = 0; i < lectures.Count; i++)
            {
                if (i + 1 < lectures.Count)
                {
                    bool isLastSeance = lectures[i].Seance % 6 == 0;
                    bool isTooFarFromNext = lectures[i + 1].Seance - lectures[i].Seance > 1;

                    if (isLastSeance && isTooFarFromNext)
                        score -= 5;
                }
                else
                {
                    score += 5;
                }
            }
        }

        return score;
    }

    private bool CheckIfExistForSecondPeriod(int lectureSeance)
    {
        var periods = _timetableService.GetSecondHalfPeriodsPerWeek();
        return periods.Contains(lectureSeance);
    }

    private bool CheckIfExistInFirstPeriod(int lectureSeance)
    {
        var periods = _timetableService.GetFirstHalfPeriodsPerWeek();
        return periods.Contains(lectureSeance);
    }
}