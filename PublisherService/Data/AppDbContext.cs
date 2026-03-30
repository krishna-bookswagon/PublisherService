using Microsoft.EntityFrameworkCore;
using PublisherService.Models;

namespace PublisherService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Publisher> Publishers { get; set; }
    }
}
