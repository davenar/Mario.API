using Mario.EF.Entities;
using Mario.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mario.Core.BusinessLayers
{
    public class MarioBusinessLayer
    {
        private readonly CourseRepository _courseRepository;
        private readonly DishRepository _dishRepository;

        public MarioBusinessLayer(CourseRepository courseRepository, DishRepository dishRepository)
        {
            _courseRepository = courseRepository;
            _dishRepository = dishRepository;
        }

        #region Courses
        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            return await _courseRepository.GetAllCoursesAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            return await _courseRepository.GetCourseByIdAsync(courseId);
        }

        public async Task AddCourseAsync(Course course)
        {
            await _courseRepository.AddCourseAsync(course);
        }

        public async Task UpdateCourseAsync(Course course)
        {
            await _courseRepository.UpdateCourseAsync(course);
        }

        public async Task DeleteCourseAsync(int courseId)
        {
            await _courseRepository.DeleteCourseAsync(courseId);
        }
        #endregion

        #region Dishes
        public async Task<IEnumerable<Dish>> GetAllDishesAsync()
        {
            return await _dishRepository.GetAllDishesAsync();
        }

        public async Task<Dish> GetDishByIdAsync(int dishId)
        {
            return await _dishRepository.GetDishByIdAsync(dishId);
        }

        public async Task AddDishAsync(Dish dish)
        {
            await _dishRepository.AddDishAsync(dish);
        }

        public async Task UpdateDishAsync(Dish dish)
        {
            await _dishRepository.UpdateDishAsync(dish);
        }

        public async Task DeleteDishAsync(int dishId)
        {
            await _dishRepository.DeleteDishAsync(dishId);
        }

        public async Task<IEnumerable<Dish>> GetDishesByCourseIdAsync(int courseId)
        {
            return await _dishRepository.GetDishesByCourseIdAsync(courseId);
        }
        #endregion
    }
}
