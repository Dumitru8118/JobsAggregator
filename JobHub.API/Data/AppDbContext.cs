using JobHub.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JobHub.API.Data
{
	public sealed class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options)
			: base(options)
		{
		}

		public DbSet<JobModel> Jobs { get; set; }
		public DbSet<JobPageModel> JobPages { get; set; }

	}
}
