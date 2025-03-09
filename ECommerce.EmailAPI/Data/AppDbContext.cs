
using ECommerce.EmailAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Service.EmailAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<EmailLogger> EmailLoggers { get; set; }
       
    }
}
