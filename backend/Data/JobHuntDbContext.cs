using Microsoft.EntityFrameworkCore;
using JobHuntBackend.Models;
using JobHuntBackend.Models.Users;
using JobHuntBackend.Models.Jobs;

namespace JobHuntBackend.Data
{
  public class JobHuntDbContext : DbContext
  {
    public JobHuntDbContext(DbContextOptions<JobHuntDbContext> options) : base(options) { }

    public DbSet<Job> Jobs { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }
    public DbSet<Interview> Interviews { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<RefreshToken>()
          .HasIndex(r => r.Token)
          .IsUnique();
    }
  }
}
