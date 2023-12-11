using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace PlaninngResolver.Domain.Entities
{
  [Table("Departements")]
  public class Departement
  {
   public int Id { get; set; }

    [Column, NotNull]
    public string Libelle { get; set; }

    [Column, NotNull]
    public int FaculteId { get; set; }

    public Faculte Faculte { get; set; }
  }
}
