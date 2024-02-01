using Mario.EF.Contexts;
using Mario.EF.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DishRepository
{
    private readonly MarioDbContext dbContext;

    public DishRepository(MarioDbContext dbContext)
    {
        this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<IEnumerable<Dish>> GetAllDishesAsync()
    {
        return await dbContext.Dishes.Include(d => d.Course).ToListAsync();
    }

    public async Task<Dish> GetDishByIdAsync(int dishId)
    {
        return await dbContext.Dishes.Include(d => d.Course).FirstOrDefaultAsync(d => d.Id == dishId);
    }

    public async Task AddDishAsync(Dish dish)
    {
        if (dish == null)
            throw new ArgumentNullException(nameof(dish));

        dbContext.Dishes.Add(dish);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateDishAsync(Dish dish)
    {
        if (dish == null)
            throw new ArgumentNullException(nameof(dish));

        dbContext.Entry(dish).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
    }

    public async Task<Dish> DeleteDishAsync(int dishId)
    {
        var dish = await dbContext.Dishes.FindAsync(dishId);
        if (dish != null)
        {
            dbContext.Dishes.Remove(dish);
            await dbContext.SaveChangesAsync();
        }

        return dish; // Return the deleted dish
    }

    public async Task<List<Dish>> GetDishesByCourseIdAsync(int courseId)
    {
        var dishes = await dbContext.Dishes
            .Include(d => d.Course)
            .Where(d => d.CourseId == courseId)
            .ToListAsync();

        return dishes;
    }

}
