using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PlaninngResolver.Domain.Entities
{
  [Table("Sections")]
  public class Section
  {
    public Section()
    {
      Groupes = new List<Groupe>();
      SalleClasses = new List<SalleClasse>();
    }

   
    public int Id { get; set; }
    //[DbColumn("Nom")]
   
    public string? Name { get; set; }
    //[DbColumn("Abréviation")]
   
    public string? Code { get; set; }

   
    public int AnneeId { get; set; }

   
    public int SpecialiteId { get; set; }

   
    public int AnneeScolaireId { get; set; }

   
    public int Semestre { get; set; }

   
    public int Nombre { get; set; }

   
    public List<Groupe> Groupes { get; set; }

   [ForeignKey("AnneeId")]
    public Year Year { get; set; }

    
    public Specialite Specialite { get; set; }
   
    public AnneeScolaire AnneeScolaire { get; set; }
  
    public List<SalleClasse> SalleClasses { get; set; }
  }
}
