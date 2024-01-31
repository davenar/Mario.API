using Mario.EF.Entities;

namespace Mario.API.Contracts.Responses
{
    public class CourseResponse
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public List<DishResponse> AvailableDishes { get; set; }
    }
}
