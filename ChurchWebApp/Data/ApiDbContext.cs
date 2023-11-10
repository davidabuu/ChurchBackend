using ChurchWebApp.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChurchWebApp.Data
{
    public class ApiDbContext :IdentityDbContext
    {

        public DbSet<EventForm> Events { get; set; }
        public ApiDbContext(DbContextOptions<ApiDbContext> options): base(options) { 
        }
    }
}
