using Asp_Net.Models;
using Microsoft.EntityFrameworkCore;

namespace Asp_Net.DataAcess
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
    }
}
