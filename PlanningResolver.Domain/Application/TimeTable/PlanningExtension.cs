using PlaninngResolver.Domain.Application.DTOs;

namespace PlaninngResolver.Domain.Application.TimeTable;

public static class PlanningExtension
{
    public static void CalculateFitness(this MultiGeneration generation)
    {
        CalculateGroupConflicts(generation);
        CalculateSectionConflicts(generation);
        CalculateTeacherConflicts(generation);
        CalculateClassRoomConflicts(generation);
    }

    private static void CalculateGroupConflicts(MultiGeneration generation)
    {
        foreach (var group in generation.Lectures.Where(x => x.GroupeId != null).GroupBy(x => x.GroupeId))
        {
            for (int i = 1; i <= 36; i++)
            {
                if (group.Count(x => x.Seance == i) > 1)
                {
                    generation.CountGroupConflict += 1;
                    generation.CountConflict += 1;
                   
                }
            }
        }
    }

    private static void CalculateSectionConflicts(MultiGeneration generation)
    {
        foreach (var section in generation.Lectures.GroupBy(x => x.SectionId))
        {
            for (int i = 1; i <= 36; i++)
            {
                var totalInSection = section.Count(x => x.Seance == i);
                var totalInGroup = section.Count(w => w.GroupeId != 0 && w.Seance == i);

                if (totalInGroup > 0 && totalInSection > totalInGroup || totalInGroup == 0 && totalInSection > 1)
                {
                    generation.CountGroupConflict += 1;
                    generation.CountConflict += 1;
                   
                }
            }
        }
    }

    private static void CalculateTeacherConflicts(MultiGeneration generation)
    {
        foreach (var teacher in generation.Lectures.GroupBy(x => x.TeacherId))
        {
            for (int i = 0; i < 36; i++)
            {
                if (teacher.Count(w => w.Seance == i) > 1)
                {
                    generation.CountTeacherConflict++;
                    generation.CountConflict++;
                    
                }
            }
        }
    }

    private static void CalculateClassRoomConflicts(MultiGeneration generation)
    {
        foreach (var room in generation.Lectures.GroupBy(x => x.ClassRoomId))
        {
            for (int i = 1; i <= 36; i++)
            {
                if (room.Count(w => w.Seance == i) > 1)
                {
                    
                    generation.CountConflict++;
                   
                }
            }
        }
    }
}