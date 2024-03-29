﻿using Mario.EF.Entities;

namespace Mario.API.Contracts.Responses
{
    public class DishResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public byte[]? Image { get; set; }
        public List<string>? Ingredients { get; set; }
        public decimal? Price { get; set; }
        public CourseResponse? Course { get; set; }
    }
}
