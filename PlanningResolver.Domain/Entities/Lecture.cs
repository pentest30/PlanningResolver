using System.ComponentModel.DataAnnotations.Schema;

namespace PlaninngResolver.Domain.Entities
{
  [Table("Lectures")]
  public class Lecture
  {
    public int Id { get; set; }
    public int TeacherId { get; set; }
    //[Column, NotNull]
    [NotMapped]
    public int ClassRoomTypeId { get; set; }
    [NotMapped]
    public int Periode { get; set; }
    public int? ClassRoomId { get; set; }
    public int CourseId { get; set; }
    public int Seance { get; set; }
    public int AnneeId { get; set; }
    public int SectionId { get; set; }
    public int? GroupeId { get; set; }
    public int FaculteId { get; set; }
    public int? DepartementId { get; set; }
    public int SpecialiteId { get; set; }
    public bool Solved { get; set; }
    public Teacher Teacher { get; set; }
    public Year Year { get; set; }
    public ClassRoom ClassRoom { get; set; }

    public Faculte Faculte { get; set; }
    public Departement Departement { get; set; }
    public Section Section { get; set; }
    public Groupe Groupe { get; set; }
    public Specialite Specialite { get; set; }
    public Course Course { get; set; }
    [NotMapped]
    public string? Display { get; set; }
    [NotMapped]
    //[Column, NotNull]
    public string? Jour { get; set; }
    [NotMapped]
    //[Column, NotNull]
    public string? Time { get; set; }

  }
}
