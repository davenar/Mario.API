namespace Mario.API.Contracts.Requests
{
    public class DishCreateRequest
    {
        public string? Name { get; set; }
        public byte[]? Image { get; set; }
        public List<string>? Ingredients { get; set; }
        public decimal? Price { get; set; }
        public int? CourseId { get; set; }
    }
}
