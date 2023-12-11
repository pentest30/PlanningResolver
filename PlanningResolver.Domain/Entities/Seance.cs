using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace PlaninngResolver.Domain.Entities
{
  [Table("Seances")]
  public class Seance
  {
     public int Id { get; set; }

   
    //[ForeignKey("AnneeScolaire")]
    public int AnneeScolaireId { get; set; }

   
    public int Number { get; set; }

   
    public int Day { get; set; }

   
    public int TeacherId { get; set; }

   
    public int Semestre { get; set; }

   // [Association(ThisKey = "TeacherId", OtherKey = "Id")]
    public Teacher Teacher { get; set; }

    //[Association(ThisKey = "AnneeScolaireId", OtherKey = "Id")]
    public AnneeScolaire AnneeScolaire { get; set; }

  }
}
