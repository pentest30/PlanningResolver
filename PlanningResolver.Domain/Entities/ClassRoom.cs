using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace PlaninngResolver.Domain.Entities
{
    [Table("ClassRooms")]
    public class ClassRoom
    {
        public ClassRoom()
        {
            SeanceLbrSalles = new List<SeanceLbrSalle>();
        }

      
        
        public int Id { get; set; }

        
        public string? Name { get; set; }
        public string? OptaCode { get; set; }
        public string? Code { get; set; }

        public string? Type { get; set; }
        public int MinSize { get; set; }
        public int MaxSize { get; set; }
        public int FaculteId { get; set; }
        
        // [DbColumn("ClassRoomTypeId")]
        public int ClassRoomTypeId { get; set; }

        public Faculte Faculte { get; set; }

        public ClassRoomType ClassRoomType { get; set; }

        public List<SeanceLbrSalle> SeanceLbrSalles { get; set; }
    }
}
