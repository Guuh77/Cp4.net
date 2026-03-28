using Microsoft.EntityFrameworkCore;
using BrinquedosAPI.Models;

namespace BrinquedosAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Brinquedo> Brinquedos { get; set; }
    }
}
