using Course4Me_ServerSide.Models;
using Microsoft.EntityFrameworkCore;

namespace Course4Me_ServerSide.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<ImageObject> Images { get; set; }    
        public DbSet<Course> Courses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VideoObject> Videos { get; set; }
        public DbSet<Episode> Episodes { get; set; }
    }
}
