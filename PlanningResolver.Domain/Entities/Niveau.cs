using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace PlaninngResolver.Domain.Entities
{
  [Table("Niveaux")]
  public class Niveau
  {
    public int Id { get; set; }

    [Column, NotNull]
    public string Libelle { get; set; }

  }
}
