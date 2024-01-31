using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario.EF.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public List<Dish> AvailableDishes { get; set; } = new List<Dish>(); // Proprietà di navigazione
    }
}

