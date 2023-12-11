using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlaninngResolver.Domain.Entities;

namespace PlaninngResolver.Domain.Infrastructure.Persistence.Configurations;

public class EntityConfiguration : IEntityTypeConfiguration<Year>,
    IEntityTypeConfiguration<Teacher>, 
    IEntityTypeConfiguration<ClassRoom>,
    IEntityTypeConfiguration<Tc>,
    IEntityTypeConfiguration<Section>,
    IEntityTypeConfiguration<ClassRoomType>,
    IEntityTypeConfiguration<SeanceLbrSalle>,
    IEntityTypeConfiguration<Faculte>,
    IEntityTypeConfiguration<AnneeScolaire>,
    IEntityTypeConfiguration<Seance>,
    IEntityTypeConfiguration<Groupe>,
    IEntityTypeConfiguration<Specialite>, IEntityTypeConfiguration<Lecture>
{
    public void Configure(EntityTypeBuilder<Year> builder)
    {
        builder.ToTable("Annees");
        //throw new NotImplementedException();
    }

    public void Configure(EntityTypeBuilder<Teacher> builder)
    {
        builder.ToTable("Teachers");
        //throw new NotImplementedException();
    }

    public void Configure(EntityTypeBuilder<ClassRoom> builder)
    {
        builder.ToTable("ClassRooms");
       // throw new NotImplementedException();
    }

    public void Configure(EntityTypeBuilder<Tc> builder)
    {
        builder.ToTable("Tcs");
        builder.HasOne(t => t.Teacher)
            .WithMany()
            .HasForeignKey(t => t.TeacherId);
        builder.HasOne(t => t.Section)
            .WithMany()
            .HasForeignKey(t => t.SectionId);
        /*builder.HasOne(t => t.Groupe)
            .WithMany()
            .HasForeignKey(t => t.GroupeId);*/
        builder.HasOne(t => t.AnneeScolaire)
            .WithMany()
            .HasForeignKey(t => t.AnneeScolaireId);
        builder.HasOne(t => t.Course)
            .WithMany()
            .HasForeignKey(t => t.CourseId);
        builder.HasOne(t => t.ClassRoomType)
            .WithMany()
            .HasForeignKey(t => t.ClassRoomTypeId);
    }

    public void Configure(EntityTypeBuilder<Section> builder)
    {
        builder.ToTable("Sections");
        builder.HasOne(t => t.Year)
            .WithMany()
            .HasForeignKey(t => t.AnneeId);
    }

    public void Configure(EntityTypeBuilder<ClassRoomType> builder)
    {
        builder.ToTable("ClassRoomTypes");
    }

    public void Configure(EntityTypeBuilder<SeanceLbrSalle> builder)
    {
        builder.ToTable("SeanceLbrSalles");
    }

    public void Configure(EntityTypeBuilder<Faculte> builder)
    {
        builder.ToTable("Facultes");
    }

    public void Configure(EntityTypeBuilder<AnneeScolaire> builder)
    {
        builder.ToTable("AnneeScolaires");
    }

    public void Configure(EntityTypeBuilder<Seance> builder)
    {
        builder.ToTable("Seances");
    }

    public void Configure(EntityTypeBuilder<Groupe> builder)
    {
        builder.ToTable("Groupes");
        /*builder.HasOne(t => t.Section)
            .WithMany()
            .HasForeignKey(t => t.SectionId);*/
    }

    public void Configure(EntityTypeBuilder<Specialite> builder)
    {
        builder.ToTable("Specialites");
        
    }

    public void Configure(EntityTypeBuilder<Lecture> builder)
    {
        builder.ToTable("Lectures");
        builder.HasOne(t => t.Year)
            .WithMany()
            .HasForeignKey(t => t.AnneeId);
    }
}