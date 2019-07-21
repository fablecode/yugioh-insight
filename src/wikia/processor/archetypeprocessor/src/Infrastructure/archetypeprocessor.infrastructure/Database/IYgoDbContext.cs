using archetypeprocessor.core.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace archetypeprocessor.infrastructure.Database
{
    public interface IYgoDbContext
    {
        DbSet<Category> Category { get; set; }
        DatabaseFacade Database { get; }
        int SaveChanges();
    }
}