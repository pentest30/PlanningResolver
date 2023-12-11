using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PlaninngResolver.Domain.Entities
{
  [Table("Courses")]
  public class Course
  {
     
    public int Id { get; set; }
    
    public string? Name { get; set; }
    
    
    public string? Code { get; set; }

    
    public string? OptaCode { get; set; }

    public int? CourseTypeId { get; set; }

    public int SpecialiteId { get; set; }

    public int AnneeId { get; set; }

    public int Semestre { get; set; }
    [ForeignKey("AnneeId")]
    public Year Year { get; set; }
    public Specialite Specialite { get; set; }
    public CourseType CourseType { get; set; }

    public int Periode { get; set; }

  }

  public enum Periode
    {
        FullDay,
        FirstHalf,
        LastHalf
    };
}
