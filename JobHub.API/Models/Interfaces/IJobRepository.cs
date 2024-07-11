using JobHub.API.Models.Database;
using JobHub.API.Models.Repository;

namespace JobHub.API.Models.Interfaces
{
    public interface IJobRepository
    {
        List<Job> GetAll();
        Job GetById(int id);
        Task SaveRange(List<Job> jobs);
        Task<PagedResponseKeyset<Job>> GetWithKeysetPagination(int reference, int pageSize);
		Task<bool> DeleteJobByIdAsync(int id);
		Task<JsonDuplicates> RemoveDuplicatesAsync();
	}
}
