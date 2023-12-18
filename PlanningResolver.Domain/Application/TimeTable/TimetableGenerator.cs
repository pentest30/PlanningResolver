using PlaninngResolver.Domain.Entities;

namespace PlaninngResolver.Domain.Application.TimeTable;

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

public class TimetableGenerator
{
    private readonly HashSet<Lecture> _timetable;
    private readonly Random _random;

    public TimetableGenerator()
    {
        _timetable = new HashSet<Lecture>();
        _random = new Random();
    }

    public List<Lecture> GenerateInitialTimetable(List<Tc> tcs, List<ClassRoom> rooms)
    {
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

                AssignSeanceAndRoom(slot, rooms);
                _timetable.Add(slot);
                Console.WriteLine
                (
                    $"Solution: count {_timetable.Count} ," +
                    $" {tc.Teacher.Nom}, {tc.Course.Name}," +
                    $"{slot.ClassRoomId}, " +
                    $"{tc.ClassRoomType.Name} " +
                    $"{slot.Seance}"
                );
            }
        }

        return new List<Lecture>(_timetable);
    }

    private void AssignSeanceAndRoom(Lecture slot, List<ClassRoom> rooms)
    {
        bool isAssigned = false;
        while (!isAssigned)
        {
            int seance = GenerateRandomSeance();
            var availableRooms = GetAvailableRooms(slot.ClassRoomTypeId, seance, rooms);

            if (!availableRooms.Any() ||
                IsTeacherBusy(slot.TeacherId, seance) ||
                IsSectionOrGroupBusy(slot.SectionId, slot.GroupeId, seance))
            {
                continue;
            }

            slot.Seance = seance;
            var room = availableRooms[_random.Next(availableRooms.Count)];
            slot.ClassRoomId = room.Id;
            slot.ClassRoom = room;
            isAssigned = true;
        }
    }

    private int GenerateRandomSeance()
    {
        byte[] randomNumber = new byte[4];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
        }
        const int totalSeances = 36;
        return BitConverter.ToInt32(randomNumber, 0) % totalSeances + 1;
    }

    private List<ClassRoom> GetAvailableRooms(int classRoomTypeId, int seance, List<ClassRoom> rooms)
    {
        return rooms.FindAll(r => r.ClassRoomTypeId == classRoomTypeId && !IsRoomBusy(r.Id, seance));
    }

    private bool IsTeacherBusy(int teacherId, int seance)
    {
        return _timetable.Any(t => t.TeacherId == teacherId && t.Seance == seance);
    }

    private bool IsRoomBusy(int roomId, int seance)
    {
        return _timetable.Any(t => t.ClassRoomId == roomId && t.Seance == seance);
    }

    private bool IsSectionOrGroupBusy(int sectionId, int? groupId, int seance)
    {
        if (groupId == null)
        {
            return _timetable.Any(lecture => lecture.SectionId == sectionId && lecture.Seance == seance);
        }

        if (_timetable.Any(lecture => lecture.SectionId == sectionId && lecture.GroupeId == null && lecture.Seance == seance))
        {
            return true;
        }

        return _timetable.Any(lecture => lecture.GroupeId == groupId && lecture.Seance == seance);
    }
}