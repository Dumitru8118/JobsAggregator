using JobHub.API.Data;

namespace JobHub.API.Helpers
{
	public static class DbInitializerExtension
	{
		public static IApplicationBuilder UseSeedDB(this IApplicationBuilder app)
		{
			ArgumentNullException.ThrowIfNull(app, nameof(app));
			using var scope = app.ApplicationServices.CreateScope();
			var services = scope.ServiceProvider;
			var context = services.GetRequiredService<AppDbContext>();
			DBSeeder.Seed(context);
			return app;
		}
	}
}
