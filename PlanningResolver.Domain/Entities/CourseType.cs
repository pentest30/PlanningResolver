using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace PlaninngResolver.Domain.Entities
{
  [Table("CourseTypes")]
    public class CourseType
    {
  
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
