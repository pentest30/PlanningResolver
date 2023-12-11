using PlaninngResolver.Domain.Entities;

namespace PlaninngResolver.Domain.Interfaces;

public interface IResolverService
{
    List<Lecture> GeneratingPlanning(int fid, int semester, int schoolYear);
}