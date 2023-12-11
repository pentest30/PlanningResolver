using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace PlaninngResolver.Domain.Entities
{
  [Table("SalleClasses")]
  public class SalleClasse
  {
    public int Id { get; set; }

    public int? SectionId { get; set; }

    public int? GroupeId { get; set; }
    
    public int ClassRoomId { get; set; }
    public Section Section { get; set; }
    public Groupe Groupe { get; set; }
    public ClassRoom ClassRoom { get; set; }
  }
}
