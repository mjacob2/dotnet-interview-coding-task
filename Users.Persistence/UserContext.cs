using Microsoft.EntityFrameworkCore;

namespace Users.Persistence;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions options) : base(options) { }

    public DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.Property(x => x.Email)
                .IsRequired();

            entity.Property(x => x.FirstName)
                .IsRequired()
                .HasMaxLength(100);
        });
    }
}