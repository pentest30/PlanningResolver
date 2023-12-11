using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace PlaninngResolver.Domain.Entities
{
    [Table("SeanceLbrSalles")]
    public class SeanceLbrSalle
    {
       
        public int Id { get; set; }

        //[ForeignKey("AnneeScolaire")]
       
        public int AnneeScolaireId { get; set; }
       
        public int Number { get; set; }
       
        public int Day { get; set; }
       
        public int SalleId { get; set; }
       
        public int Semestre { get; set; }
       
        public ClassRoom Salle { get; set; }
        public AnneeScolaire AnneeScolaire { get; set; }
    }
}
