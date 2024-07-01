namespace JobHub.API.Dtos.Response
{
    public record PagedResponseKeysetDto<T>
    {
        public int Reference { get; init; }
        public List<T> Data { get; init; }
    }
}
