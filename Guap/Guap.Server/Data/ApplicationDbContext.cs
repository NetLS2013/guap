using Guap.Server.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Guap.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public ApplicationDbContext(DbContextOptions options)
            : base (options)
        {
            
        }
    }
}