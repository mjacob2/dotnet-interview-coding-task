using Microsoft.EntityFrameworkCore;

namespace Users.Persistence;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions options) : base(options) { }

    public DbSet<UserProfile> UserProfiles { get; set; }
}