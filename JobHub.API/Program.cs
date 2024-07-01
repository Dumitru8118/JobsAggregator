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
using Newtonsoft.Json.Serialization;
using System.Text;


internal class Program
{
	private static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.


		builder.Services.AddControllers(options =>
						options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true)
					.AddNewtonsoftJson(options =>
						options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
					);

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
				});
			}
		});


		var serviceProvider = builder.Services.BuildServiceProvider();
		var logger = serviceProvider.GetRequiredService<ILogger<ControllerBase>>();
		builder.Services.AddSingleton(typeof(ILogger), logger);

		builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


		var jwtSettings = builder.Configuration.GetSection("Jwt");

		builder.Services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(o =>
		{
			o.TokenValidationParameters = new TokenValidationParameters
			{
				ValidIssuer = builder.Configuration["Jwt:Issuer"],
				ValidAudience = builder.Configuration["Jwt:Audience"],
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])),
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = false,
				ValidateIssuerSigningKey = true
			};
		});

		builder.Services.AddCors(policyBuilder =>
			policyBuilder.AddDefaultPolicy(policy =>
				policy.WithOrigins("*")
				.AllowAnyHeader()
				.AllowAnyHeader())
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

		app.UseAuthorization();
		app.UseAuthentication();
		app.UseAuthorization();

		app.MapControllers();

		app.Run();
	}
}