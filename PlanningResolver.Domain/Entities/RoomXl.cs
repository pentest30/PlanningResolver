namespace PlaninngResolver.Domain.Entities
{
    public class RoomXl
    {
        //[DbColumn("Id")]
        //[PrimaryKey, NotNull]
        public int Id { get; set; }

        //[Column, NotNull]
         //[DbColumn("Name")]
        public string Name { get; set; }

        //[DbColumn("Code")]
        //[Column, NotNull]
        public string Code { get; set; }
    }

}
