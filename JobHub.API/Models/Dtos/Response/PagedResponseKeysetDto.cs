namespace JobHub.API.Models.Dtos.Response
{
    public record PagedResponseKeysetDto<T>
    {
        public string Reference { get; init; }
        public List<T> Data { get; init; }
    }
}
