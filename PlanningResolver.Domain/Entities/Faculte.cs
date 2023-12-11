using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace PlaninngResolver.Domain.Entities
{
  [Table("Facultes")]
  public class Faculte
  {
    public int Id { get; set; }

    public string? Libelle { get; set; }

  }
}
