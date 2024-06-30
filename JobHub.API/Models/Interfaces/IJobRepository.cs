namespace JobHub.API.Models.Interfaces
{
    public interface IJobRepository
    {
        List<JobModel> GetAll();
        JobModel GetById(int id);
        void Add(JobModel job);
        void Update(JobModel job);
        void Delete(JobModel job);
        Task SaveRange(List<JobModel> jobs);
        Task<PagedResponseKeyset<JobModel>> GetWithKeysetPagination(int reference, int pageSize);
    }
}
