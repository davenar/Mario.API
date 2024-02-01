using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario.EF.Entities
{
    public class Dish
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public byte[]? Image { get; set; }
        public decimal? Price { get; set; }
        public int? CourseId { get; set; }
        public Course? Course { get; set; }

        [NotMapped] // Indica a EF Core di non mappare questa proprietà al database
        public List<string>? Ingredients { get; set; }
        // Proprietà calcolata per mappare la stringa degli ingredienti al database
        public string IngredientsAsString
        {
            get => Ingredients != null ? string.Join(",", Ingredients) : null;
            set => Ingredients = value?.Split(',').ToList();
        }
    }
}
