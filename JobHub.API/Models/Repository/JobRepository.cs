﻿using JobHub.API.Data;
using JobHub.API.Models.Database;
using JobHub.API.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing;

namespace JobHub.API.Models.Repository
{
    public class JsonDuplicates
    {
        public bool Succes { get; set; }
        public int rowsAffected { get; set; }

        public JsonDuplicates()
        {

        }
    }

    public class JobRepository : IJobRepository
	{
		private readonly AppDbContext _context;

		public JobRepository(AppDbContext context)
		{
			_context = context;
		}

		public  List<Job> GetAll()
		{
			return _context.Jobs.Select(job => new Job()
			{
				Url = job.Url,
				CompanyName = job.CompanyName,
				JobName = job.JobName,
				DatePosted = job.DatePosted,
			}).ToList();
		}

		public Job GetById(int id)
		{
			throw new NotImplementedException();
		}

		public void Update(Job job)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Save Range of Jobs in Batches of 100 jobs.
		/// </summary>
		/// <param name="jobs">List of jobs to be saved. </param>
		/// <exception cref="ArgumentException"></exception>
		public async Task SaveRange(List<Job> jobs)
		{
			if (jobs == null || !jobs.Any())
			{
				throw new ArgumentException("The products collection is null or empty.");
			}

			//await _context.Jobs.AddRangeAsync(jobs);
			IEnumerable<Job> listOfJobs = jobs;

			int batchSize = 100;

			for (int i = 0; i < listOfJobs.ToList().Count; i += batchSize)
			{
				IEnumerable<Job> batch = listOfJobs.Skip(i).Take(batchSize).ToList();
				await _context.Jobs.AddRangeAsync(batch);
			}
			_context.SaveChanges();
		}

		public async Task<PagedResponseKeyset<Job>> GetWithKeysetPagination(int reference, int pageSize)
		{
			var jobs = await _context.Jobs.AsNoTracking()
				.OrderBy(x => x.Id)
				.Where(p => p.Id > reference)
				.Take(pageSize)
				.ToListAsync();

			var newReference = jobs.Count != 0 ? jobs.Last().Id :0;

			var pagedResponse = new PagedResponseKeyset<Job>(jobs, newReference);

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

         public async Task<JsonDuplicates> RemoveDuplicatesAsync()
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

                    JsonDuplicates jres = new JsonDuplicates();
                    jres.rowsAffected = rowsAffected;
					jres.Succes = true;

                    return jres;
				}
				else
				{
                    JsonDuplicates jres = new JsonDuplicates();
                    jres.rowsAffected = 0;
                    jres.Succes = false;
                    return jres; // No duplicates found
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
