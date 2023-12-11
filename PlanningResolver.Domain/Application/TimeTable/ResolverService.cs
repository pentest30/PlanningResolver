using Microsoft.EntityFrameworkCore;
using PlaninngResolver.Domain.Application.DTOs;
using PlaninngResolver.Domain.Entities;
using PlaninngResolver.Domain.Interfaces;

namespace PlaninngResolver.Domain.Application.Rules;

public class ResolverService : IResolverService
{
    public ResolverService(
        IRepository<Tc> lectureRepository,
        IRepository<ClassRoom> classRoomRepository,
        IRepository<Lecture> lectureRepo)
    {
        _lectureRepository = lectureRepository;
        _classRoomRepository = classRoomRepository;
        _lectureRepo = lectureRepo;
    }

    private readonly IRepository<ClassRoom> _classRoomRepository;
    private readonly IRepository<Lecture> _lectureRepo;
    private readonly IRepository<Tc> _lectureRepository;

    public  List<Lecture> GeneratingPlanning(int fid, int semester, int schoolYear)
    {
        var rooms = GetClassRooms(fid);
        var tcs = GetLecturesToList(fid, semester, schoolYear);
        var generator = new TimetableGenerator();
        var planing= generator.GenerateInitialTimetable(tcs, rooms);
        var sol = new MultiGeneration();
        sol.Lectures = planing;
        sol.CalculateFitness();
        var simulation = new TimetableSearch(planing, rooms); 
        simulation.Run();
        Console.WriteLine(simulation.GetBestScore());
        foreach (var lecture in simulation.GetBestTimetable())
        {
            _lectureRepo.AddAsync(lecture).GetAwaiter().GetResult();
        }

        _lectureRepo.UnitOfWork.SaveChangesAsync().GetAwaiter().GetResult();
        return planing;
    }
    private List<ClassRoom> GetClassRooms(int fid)
    {
        var rooms = _classRoomRepository
            .GetAll()
            .Include(x=>x.SeanceLbrSalles)
            .Where(x => x.FaculteId == fid)
            .ToList();
        return rooms;
    }
    private List<Tc> GetLecturesToList(int fid, int semester, int schoolYear)
    {
        return _lectureRepository
            .GetAll()
            .Include(x => x.Section)
            .Include(x => x.Section.Specialite)
            .Include(x => x.Section.Year)
            .Include(x => x.Groupe)
            .Include(x => x.Course)
            .Include(x => x.ClassRoomType)
            .Include(x => x.Teacher)
            .Include(x => x.Teacher.Seances)
            .OrderBy(x => x.ClassRoomTypeId)
            .ThenByDescending(x => x.Periode)
            .ThenBy(x => x.GroupeId).Where(
                x => x.Section.Specialite.FaculteId == fid &&
                     x.Semestre == semester
                     && x.Section.AnneeScolaireId == schoolYear)
            .ToList();
    }
    
}