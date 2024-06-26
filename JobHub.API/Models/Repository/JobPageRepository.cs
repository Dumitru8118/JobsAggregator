using JobHub.API.Data;
using JobHub.API.Models.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace JobHub.API.Models.Repository
{
	public class JobPageRepository : IJobPageRepository
	{
		private readonly AppDbContext _context;

		public JobPageRepository(AppDbContext context)
		{
			_context = context;
		}

		public void Add(JobPageModel job)
		{
			throw new NotImplementedException();
		}

		public void Delete(JobPageModel job)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<JobPageModel> GetAll()
		{
			throw new NotImplementedException();
		}

		public JobPageModel GetById(int id)
		{
			throw new NotImplementedException();
		}

		public void Update(JobPageModel job)
		{
			throw new NotImplementedException();
		}

		public async Task SaveRange(List<JobPageModel> jobPages)
		{
			if (jobPages == null || !jobPages.Any())
			{
				throw new ArgumentException("The products collection is null or empty.");
			}

			//await _context.Jobs.AddRangeAsync(jobs);
			IEnumerable<JobPageModel> listOfPages = jobPages;

			int batchSize = 100;

			for (int i = 0; i < listOfPages.ToList().Count; i += batchSize)
			{
				IEnumerable<JobPageModel> batch = listOfPages.Skip(i).Take(batchSize).ToList();
				await _context.JobPages.AddRangeAsync(batch);
			}

			_context.SaveChanges();
		}
	}
}
