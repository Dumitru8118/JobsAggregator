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

		public void Add(JobModel job)
		{
			throw new NotImplementedException();
		}

		public void Delete(JobModel job)
		{
			throw new NotImplementedException();
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

	}
}
