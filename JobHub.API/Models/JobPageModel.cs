using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobHub.API.Models
{
	public class JobPageModel
	{
		// Additional properties
		[Key, ForeignKey("JobModel")]
		public int Id { get; set; } // Use the same ID as JobModel

		[Required]
		public string Salary { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public string JobType { get; set; }

		[Required]
		public string ExpertiseLvl { get; set; }

		[Required]
		public string Industry { get; set; }

		[Required]
		public string QualificationLvl { get; set; }

		[Required]
		public string Department { get; set; }

		[Required]
		public string Language { get; set; }

		public JobPageModel(
			int id,
			string salary, 
			string city, 
			string jobType, 
			string expertiseLvl,
			string industry, 
			string qualificationLvl, 
			string department, 
			string language)
		{
			Id = id;
			Salary = salary;
			City = city;
			JobType = jobType;
			ExpertiseLvl = expertiseLvl;
			Industry = industry;
			QualificationLvl = qualificationLvl;
			Department = department;
			Language = language;
		}
	}
}
