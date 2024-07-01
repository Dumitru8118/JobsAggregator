namespace JobHub.API.Models.Repository.IRepository
{
	public interface IJobPageRepository
	{
		IEnumerable<JobPageModel> GetAll();
		JobPageModel GetById(int id);
		void Add(JobPageModel jobP);
		void Update(JobPageModel jobP);
		void Delete(JobPageModel jobP);
		Task SaveRange(List<JobPageModel> jobP);
	}
}
