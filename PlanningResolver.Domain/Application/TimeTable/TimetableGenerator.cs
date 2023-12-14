using PlaninngResolver.Domain.Entities;

namespace PlaninngResolver.Domain.Application.Rules;

public class TimetableGenerator
{
    public List<Lecture> GenerateInitialTimetable(List<Tc> tcs, List<ClassRoom> rooms)
    {
        var timetable = new List<Lecture>();
        var random = new Random();

        foreach (var tc in tcs)
        {
            for (int i = 0; i < tc.ScheduleWieght; i++)
            {
                var slot = new Lecture
                {
                    CourseId = tc.CourseId,
                    TeacherId = tc.TeacherId,
                    SectionId = tc.SectionId,
                    GroupeId = tc.GroupeId,
                    ClassRoomTypeId = tc.ClassRoomTypeId,
                    Teacher = tc.Teacher,
                    SpecialiteId = tc.Course.SpecialiteId,
                    FaculteId = tc.Teacher.FaculteId,
                };

                AssignSeanceAndRoom(slot, tc, rooms, timetable, random);
                timetable.Add(slot);
                Console.WriteLine($"Solution: count {timetable.Count} , {tc.Teacher.Nom}, {tc.Course.Name},{slot.ClassRoomId}, {tc.ClassRoomType.Name} {slot.Seance}");

            }
        }

        return timetable;
    }

    private void AssignSeanceAndRoom(Lecture slot, Tc tc, List<ClassRoom> rooms, List<Lecture> timetable, Random random)
    {
        bool isAssigned = false;
        while (!isAssigned)
        {
            int seance = random.Next(1, 37); // Assuming there are 36 seances
            var availableRooms = rooms.Where(r => r.ClassRoomTypeId == tc.ClassRoomTypeId&& !IsRoomBusy(r.Id, seance, timetable)).ToList();

            if (!availableRooms.Any() ||
                IsTeacherBusy(tc.TeacherId, seance, timetable)
                || IsSectionOrGroupBusy(tc.SectionId, tc.GroupeId, seance, timetable)) continue;
            slot.Seance = seance;
            var room = availableRooms[random.Next(availableRooms.Count)];
            slot.ClassRoomId = room.Id;
            slot.ClassRoom = room;
            isAssigned = true;
        }
    }

    private bool IsTeacherBusy(int teacherId, int seance, List<Lecture> timetable)
    {
        return timetable.Any(t => t.TeacherId == teacherId && t.Seance == seance);
    }

    private bool IsRoomBusy(int roomId, int seance, List<Lecture> timetable)
    {
        return timetable.Any(t => t.ClassRoomId == roomId && t.Seance == seance);
    }
    private bool IsSectionOrGroupBusy(int sectionId, int? groupId, int seance, List<Lecture> timetable)
    {
        if (groupId == null)
        {
            // Check for any lecture in the section (without specific group) occupying the seance
            return timetable.Any(lecture => lecture.SectionId == sectionId && lecture.Seance == seance);
        }

        // Check for any section-wide lecture or specific group lecture occupying the seance
        if (timetable.Any(
                lecture =>
                    (lecture.SectionId == sectionId && lecture.GroupeId == null) && lecture.Seance == seance))
        {
            return true;
        }
        return timetable.Any(lecture => lecture.GroupeId == groupId && lecture.Seance == seance);
    }
}