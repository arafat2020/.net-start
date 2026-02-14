using Microsoft.EntityFrameworkCore;
using PaymentApi.Models;

namespace PaymentApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
