using PlaninngResolver.Domain.Entities;

namespace PlaninngResolver.Domain.Application.DTOs;

public class MultiGeneration
{
    public List<Lecture> Lectures { get; set; }
    public int CountConflict { get; set; }
    public int CountTeacherConflict { get; set; }
    public int CountGroupConflict { get; set; }
}