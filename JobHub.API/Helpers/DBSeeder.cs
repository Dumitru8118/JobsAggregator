using JobHub.API.Data;
using JobHub.API.Models.Database;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using BC = BCrypt.Net.BCrypt;

namespace JobHub.API.Helpers
{
	public class DBSeeder
	{
		public static void Seed(AppDbContext dbContext)
		{
			ArgumentNullException.ThrowIfNull(dbContext, nameof(dbContext));
			dbContext.Database.EnsureCreated();

			var executionStrategy = dbContext.Database.CreateExecutionStrategy();

			executionStrategy.Execute(
			  () => {
				  using (var transaction = dbContext.Database.BeginTransaction())
				  {
					  try
					  {
						  // Seed Users
						  if (!dbContext.Users.Any())
						  {
							  var usersData = File.ReadAllText("./Resources/users.json");
							  var parsedUsers = JsonConvert.DeserializeObject<User[]>(usersData);

							  foreach (var user in parsedUsers)
							  {
								  user.Password = BC.HashPassword(user.Password);
							  }

							  dbContext.Users.AddRange(parsedUsers);
							  dbContext.SaveChanges();
						  }

						  transaction.Commit();
					  }
					  catch (Exception ex)
					  {
						  transaction.Rollback();
					  }
				  }
			  });
		}
	}
}
