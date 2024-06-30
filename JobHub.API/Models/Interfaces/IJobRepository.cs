namespace JobHub.API.Models.Interfaces
{
    public interface IJobRepository
    {
        List<JobModel> GetAll();
        JobModel GetById(int id);
        Task SaveRange(List<JobModel> jobs);
        Task<PagedResponseKeyset<JobModel>> GetWithKeysetPagination(int reference, int pageSize);
		Task<bool> DeleteJobByIdAsync(int id);
		Task<(bool Success, int DeletedCount)> RemoveDuplicatesAsync();
	}
}
