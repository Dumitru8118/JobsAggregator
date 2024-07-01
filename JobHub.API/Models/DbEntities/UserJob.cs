namespace JobHub.API.Models.Database
{
    public class UserJob
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int JobId { get; set; }
        public Job Job { get; set; }

        // Flag to indicate if the job is liked by the user
        public bool Liked { get; set; }
    }
}
