using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace PlaninngResolver.Domain.Entities
{
  [Table("Fillieres")]


  public class Filliere
  {
    public int Id { get; set; }
    
    public string? Libelle { get; set; }

  }
}
