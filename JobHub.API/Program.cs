using JobHub.API.Data;
using JobHub.API.Filters;
using JobHub.API.Helpers;
using JobHub.API.Models.Interfaces;
using JobHub.API.Models.Repository;
using JobHub.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Text;


internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(option =>
			{
				option.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Northwind CRUD",
					Version = "v1"
				});
				option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter a valid token",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "bearer"
				});
				//option.AddSecurityRequirement(new OpenApiSecurityRequirement
				//{
				//	{
				//		new OpenApiSecurityScheme
				//		{
				//			Reference = new OpenApiReference
				//			{
				//				Type = ReferenceType.SecurityScheme,
				//				Id = "Bearer"
				//			}
				//		},
				//		new string[] {}
				//	}
				//});
				option.OperationFilter<AuthResponsesOperationFilter>();
			}
		);


		var cnnString = builder.Configuration.GetConnectionString("JobsDb");
		var dbProvider = builder.Configuration.GetConnectionString("Provider");

		builder.Services.AddDbContext<AppDbContext>(options =>
		{
			if (dbProvider == "Postgres")
			{
				options.UseNpgsql(cnnString, b =>
				{
					b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName); // If using migrations
					b.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
				})
				.EnableDetailedErrors()
                .EnableSensitiveDataLogging();
			}
		});


		var serviceProvider = builder.Services.BuildServiceProvider();
		var logger = serviceProvider.GetRequiredService<ILogger<ControllerBase>>();
		builder.Services.AddSingleton(typeof(ILogger), logger);

		builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());



		var jwtSettings = builder.Configuration.GetSection("Jwt");
		var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

		builder.Services.AddAuthentication(x =>
		{
			x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(x =>
		{
			x.RequireHttpsMetadata = false;
			x.SaveToken = true;
			x.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false,
				ValidateAudience = false
			};

			// Add events to log errors
			x.Events = new JwtBearerEvents
			{
				OnAuthenticationFailed = context =>
				{
					// Log the exception message
					var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

					Console.WriteLine($"Authentication failed: {context.Exception.Message}");
					Console.WriteLine($"JWT Token: {token}");
					if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
					{
						context.Response.Headers.Add("Token-Expired", "true");
					}
					return Task.CompletedTask;
				},
				OnChallenge = context =>
				{
					// Log challenge details
					var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
					Console.WriteLine($"OnChallenge error: {context.Error}, description: {context.ErrorDescription}");
					Console.WriteLine($"JWT Token: {token}");
					return Task.CompletedTask;
				}
			};
		});
		// Register TokenService with the secret from configuration
		builder.Services.AddSingleton(new TokenService(jwtSettings["Key"]));

		builder.Services.AddCors(options =>
		{
			options.AddDefaultPolicy(policy =>
			{
				policy.AllowAnyOrigin()
					  .AllowAnyMethod()
					  .AllowAnyHeader();
			});
		});


		// Add services to the container.
		builder.Services.AddControllers(options =>
						options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
					.AddNewtonsoftJson(options =>
						options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
					);

		// Declared services
		builder.Services.AddScoped<IJobRepository, JobRepository>();
		builder.Services.AddScoped<DBSeeder>();
		builder.Services.AddTransient<AuthService>();



		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
			app.UseSeedDB();
		}



		app.UseCors();

		app.UseHttpsRedirection();

		app.UseAuthentication();
		app.UseAuthorization();


		app.MapControllers();

		app.Run();
	}
}