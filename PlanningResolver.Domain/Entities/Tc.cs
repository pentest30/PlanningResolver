using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlaninngResolver.Domain.Entities;

[Table("Tcs")]
public class Tc
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int TeacherId { get; set; }

    [Required]
    public int CourseId { get; set; }

    [Required]
    public int ScheduleWieght { get; set; }

    [Required]
    public int AnneeScolaireId { get; set; }

    [Required]
    public int Semestre { get; set; }

    [Required]
    public int ClassRoomTypeId { get; set; }

    [Required]
    public int Periode { get; set; }

    [Required]
    public int SectionId { get; set; }

    public int? GroupeId { get; set; }

    // Navigation properties (uncomment and modify as per your related entities)
    public Teacher Teacher { get; set; }
     public Course Course { get; set; }
     public AnneeScolaire AnneeScolaire { get; set; }
     public Section Section { get; set; }
     public Groupe Groupe { get; set; }
     public ClassRoomType ClassRoomType { get; set; }
}