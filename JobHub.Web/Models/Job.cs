namespace JobHub.Web.Models
{
    public class Job
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public string Company { get; set; }
        public string JobTitle { get; set; }
        public string DatePosted { get; set; }
    }
}
