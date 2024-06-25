namespace JobHub.API.Models.Repository.IRepository
{
	public interface IJobRepository
	{
		IEnumerable<JobModel> GetAll();
		JobModel GetById(int id);
		void Add(JobModel job);
		void Update(JobModel job);
		void Delete(JobModel job);
		Task SaveRange(List<JobModel> jobs);
	}
}
