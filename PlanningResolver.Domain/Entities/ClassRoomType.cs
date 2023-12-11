using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace PlaninngResolver.Domain.Entities
{
  [Table("ClassRoomTypes")]
  public class ClassRoomType
  {
    
    public int Id { get; set; }

    [Column, NotNull]
    public string Name { get; set; }
  }
}
