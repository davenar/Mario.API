using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario.EF.Repositories
{
    using Mario.EF.Contexts;
    using Mario.EF.Entities;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CourseRepository
    {
        private readonly MarioDbContext dbContext;

        public CourseRepository(MarioDbContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await dbContext.Courses.ToListAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            return await dbContext.Courses.FindAsync(courseId);
        }

        public async Task AddCourseAsync(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            dbContext.Courses.Add(course);
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateCourseAsync(Course course)
        {
            if (course == null)
                throw new ArgumentNullException(nameof(course));

            dbContext.Entry(course).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteCourseAsync(int courseId)
        {
            var course = await dbContext.Courses.FindAsync(courseId);
            if (course != null)
            {
                dbContext.Courses.Remove(course);
                await dbContext.SaveChangesAsync();
            }
        }

        //public async Task<IEnumerable<Dish>> GetDishesByCourseIdAsync(int courseId)
        //{
        //    var course = await dbContext.Courses
        //        .Include(c => c.AvailableDishes)
        //        .FirstOrDefaultAsync(c => c.Id == courseId);

        //    return course?.AvailableDishes.ToList() ?? new List<Dish>();
        //}
    }

}
