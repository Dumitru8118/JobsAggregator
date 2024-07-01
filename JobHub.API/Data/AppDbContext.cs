using JobHub.API.Models;
using JobHub.API.Models.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.DevTools;

namespace JobHub.API.Data
{
    public sealed class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}

		public DbSet<Job> Jobs { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<UserJob> UserJobs { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<User>()
			  .HasKey(u => new {
				  u.UserId
			  });
			builder.Entity<Job>()
			  .HasKey(e => new {
				  e.Id
			  });
			builder.Entity<UserJob>()
			.HasKey(ue => new {
				  ue.UserId,
				  ue.JobId
			  });
			builder.Entity<UserJob>()
			  .HasOne(ue => ue.User)
			  .WithMany(user => user.UserJobs)
			  .HasForeignKey(u => u.UserId);
			builder.Entity<UserJob>()
			  .HasOne(uc => uc.Job)
			  .WithMany(ev => ev.UserJobs)
			  .HasForeignKey(ev => ev.UserId);
		}

	}
}
