using System.ComponentModel.DataAnnotations;

namespace JobHub.API.Models.Dtos.Request
{
	public class UserJobInputModel
	{
		[Required]
		public string UserId { get; set; }

		[Required]
		public string JobId { get; set; }
	}
}
