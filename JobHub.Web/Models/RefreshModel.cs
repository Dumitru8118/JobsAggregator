namespace JobHub.Web.Models
{
    public class RefreshModel
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
