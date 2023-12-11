using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace PlaninngResolver.Domain.Entities
{
    [Table("ClassSeances")]
  public class ClassSeance
  {
   
    public int Id { get; set; }

    [Column, NotNull]
    public int ClassRoomId { get; set; }

   
    public ClassRoom ClassRoom { get; set; }

   
    public ClassRoomType ClassRoomType { get; set; }

    
    public AnneeScolaire AnneeScolaire { get; set; }

    [Column, NotNull]
    public int Seance { get; set; }
    public string TypeClass { get; set; }
    public int Min { get; set; }
    public int Max { get; set; }

    public string Jour { get; set; }

    public string Time { get; set; }

    [Column, NotNull]
    public int ClassRoomTypeId { get; set; }

    [Column, NotNull]
    public int Semestre { get; set; }

    [Column, NotNull]
    public int AnneeScolaireId { get; set; }


    public static List<ClassSeance> GenerateSeances(List<ClassRoom> classRooms, int anneeScolaire, int semestre)
    {
      var result = new List<ClassSeance>();
      // List<SeanceLbrSalle> ss;
      //    var db = new DbModel();

      foreach (var classRoom in classRooms)
      {

        for (int i = 1; i < 37; i++)
        {
          var item = new ClassSeance
          {
            ClassRoomId = classRoom.Id,
            TypeClass = classRoom.ClassRoomType.Name,
            ClassRoomTypeId = Convert.ToInt32(classRoom.ClassRoomTypeId),
            Seance = i,
            Min = classRoom.MinSize,
            Max = classRoom.MaxSize,
            AnneeScolaireId = anneeScolaire,
            Semestre = semestre
          };
          // if (item.TypeClass=="Amphi"&& item.Seance==36 ) continue;
          result.Add(item);
        }


      }
      return result;
    }
  }
}