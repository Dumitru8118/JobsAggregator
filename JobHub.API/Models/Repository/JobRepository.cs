using JobHub.API.Data;
using JobHub.API.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing;

namespace JobHub.API.Models.Repository
{
    public class JobRepository : IJobRepository
	{
		private readonly AppDbContext _context;

		public JobRepository(AppDbContext context)
		{
			_context = context;
		}

		public  List<JobModel> GetAll()
		{
			return _context.Jobs.Select(job => new JobModel()
			{
				Url = job.Url,
				CompanyName = job.CompanyName,
				JobName = job.JobName,
				DatePosted = job.DatePosted,
			}).ToList();
		}

		public JobModel GetById(int id)
		{
			throw new NotImplementedException();
		}

		public void Update(JobModel job)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Save Range of Jobs in Batches of 100 jobs.
		/// </summary>
		/// <param name="jobs">List of jobs to be saved. </param>
		/// <exception cref="ArgumentException"></exception>
		public async Task SaveRange(List<JobModel> jobs)
		{
			if (jobs == null || !jobs.Any())
			{
				throw new ArgumentException("The products collection is null or empty.");
			}

			//await _context.Jobs.AddRangeAsync(jobs);
			IEnumerable<JobModel> listOfJobs = jobs;

			int batchSize = 100;

			for (int i = 0; i < listOfJobs.ToList().Count; i += batchSize)
			{
				IEnumerable<JobModel> batch = listOfJobs.Skip(i).Take(batchSize).ToList();
				await _context.Jobs.AddRangeAsync(batch);
			}
			_context.SaveChanges();
		}

		public async Task<PagedResponseKeyset<JobModel>> GetWithKeysetPagination(int reference, int pageSize)
		{
			var jobs = await _context.Jobs.AsNoTracking()
				.OrderBy(x => x.Id)
				.Where(p => p.Id > reference)
				.Take(pageSize)
				.ToListAsync();

			var newReference = jobs.Count != 0 ? jobs.Last().Id : 0;

			var pagedResponse = new PagedResponseKeyset<JobModel>(jobs, newReference);

			return pagedResponse;
		}
		public async Task<bool> DeleteJobByIdAsync(int id)
		{
			var job = await _context.Jobs.FindAsync(id);
			if (job == null)
			{
				return false; // Job with the specified ID not found
			}

			_context.Jobs.Remove(job);
			await _context.SaveChangesAsync();
			return true; // Job successfully deleted
		}

		public async Task<(bool Success, int DeletedCount)> RemoveDuplicatesAsync()
		{
			var duplicatesQuery = @"
				DELETE FROM ""Jobs""
				WHERE ctid NOT IN (
					SELECT MIN(ctid)
					FROM ""Jobs""
					GROUP BY ""Url""
				);
			";

			try
			{
				int rowsAffected = await _context.Database.ExecuteSqlRawAsync(duplicatesQuery);

				// If rowsAffected > 0, duplicates were deleted
				// If rowsAffected == 0, no duplicates were found

				if (rowsAffected > 0)
				{
					return (true, rowsAffected);
				}
				else
				{
					return (false, rowsAffected); // No duplicates found
				}
			}
			catch (Exception ex)
			{
				// Log the exception or handle it accordingly
				throw new Exception("Failed to remove duplicates", ex);
			}
		}

	}
}
