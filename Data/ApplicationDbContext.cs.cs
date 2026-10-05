using Microsoft.EntityFrameworkCore;
using websiteCofee.Models;

namespace websiteCofee.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Kani wuxuu SQL Server ka dhex abuurayaa khaanad la yiraahdo Admins
        // Waxaan u beddelnay 'Admin' oo ah class-ka rasmiga ah ee database-ka
        public DbSet<Admin> Admins { get; set; }
    }
}
