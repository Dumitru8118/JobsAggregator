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
	}

	//public class AppDbContext : DbContext
	//{
	//	protected private readonly IConfiguration Configuration;

	//	public AppDbContext(IConfiguration configuration)
	//	{
	//		Configuration = configuration;
	//	}

	//	protected override void OnConfiguring(DbContextOptionsBuilder options)
	//	{
	//		options.UseNpgsql(Configuration.GetConnectionString("JobsDb"));
	//	}

	//	DbSet<JobModel> Jobs { get; set; }
	//}
}
