using System.ComponentModel.DataAnnotations.Schema;

namespace PlaninngResolver.Domain.Entities
{
  [Table("Groupes")]
  public class Groupe
  {
    public Groupe()
    {
      SalleClasses = new List<SalleClasse>();
    }

   
    public int Id { get; set; }

    
    public string? Name { get; set; }

   
    public int SectionId { get; set; }

   
    public int Semestre { get; set; }

   
    public int Nombre { get; set; }

    public Section Section { get; set; }
   
    public string? Code { get; set; }
    public List<SalleClasse> SalleClasses { get; set; }
  }
}
