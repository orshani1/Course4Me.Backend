using Course4Me_ServerSide.Models;
using Microsoft.EntityFrameworkCore;

namespace Course4Me_ServerSide.Data
{
    public interface IAppDbContext
    {
        DbSet<Course> Courses { get; set; }
        DbSet<User> Users { get; set; }
    }
}