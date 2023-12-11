using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PlaninngResolver.Domain.Interfaces;

namespace PlaninngResolver.Domain.Infrastructure.Persistence;

public class PlanningDbContext : DbContext, IUnitOfWork
{

    public PlanningDbContext(DbContextOptions<PlanningDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
  
}