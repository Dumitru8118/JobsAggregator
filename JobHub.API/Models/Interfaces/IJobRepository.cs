using JobHub.API.Models.Database;

namespace JobHub.API.Models.Interfaces
{
    public interface IJobRepository
    {
        List<Job> GetAll();
        Job GetById(int id);
        Task SaveRange(List<Job> jobs);
        Task<PagedResponseKeyset<Job>> GetWithKeysetPagination(int reference, int pageSize);
		Task<bool> DeleteJobByIdAsync(int id);
		Task<(bool Success, int DeletedCount)> RemoveDuplicatesAsync();
	}
}
