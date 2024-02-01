using Mario.API.Contracts.Requests;
using Mario.API.Contracts.Responses;
using Mario.Core.BusinessLayers;
using Mario.EF.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mario.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly MarioBusinessLayer _marioBusinessLayer;

        public DishesController(MarioBusinessLayer marioBusinessLayer)
        {
            _marioBusinessLayer = marioBusinessLayer;
        }


        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(IEnumerable<DishResponse>))]
        [Route("FetchAllDishes")]
        public async Task<IActionResult> FetchAllDishes()
        {
            var data = await _marioBusinessLayer.GetAllDishesAsync();

            var response = data.Select(x => new DishResponse
            {
                Id = x.Id,
                Course = x?.Course is not null ? new CourseResponse { Id = x.Course.Id, Description = x?.Course?.Description} : null,
                Image = x.Image,
                Ingredients = x?.Ingredients ?? new(),
                Name = x.Name,
                Price = x.Price
            }).ToList();

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(Dish))]
        [Route("GetDishById/{dishId}")]
        public async Task<IActionResult> GetDishById(int dishId)
        {
            var dish = await _marioBusinessLayer.GetDishByIdAsync(dishId);

            if (dish == null)
            {
                return NotFound();
            }

            return Ok(dish);
        }

        [HttpPost]
        [ProducesResponseType(statusCode: 201, type: typeof(Dish))]
        [Route("CreateDish")]
        public async Task<IActionResult> CreateDish([FromBody] DishCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var dish = new Dish
            {
                // Set properties based on the DishCreateRequest model
                Name = request.Name,
                // Add other properties as needed
            };

            await _marioBusinessLayer.AddDishAsync(dish);

            return CreatedAtAction(nameof(FetchAllDishes), new { id = dish.Id }, dish);
        }

        [HttpPut]
        [ProducesResponseType(statusCode: 200, type: typeof(Dish))]
        [Route("UpdateDish")]
        public async Task<IActionResult> UpdateDish([FromBody] DishUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDish = await _marioBusinessLayer.GetDishByIdAsync(request.Id);

            if (existingDish == null)
            {
                return NotFound();
            }

            // Update existingDish properties based on the DishUpdateRequest model
            existingDish.Name = request.Name;
            // Update other properties as needed

            await _marioBusinessLayer.UpdateDishAsync(existingDish);

            return Ok(existingDish);
        }

        [HttpDelete]
        [ProducesResponseType(statusCode: 200, type: typeof(Dish))]
        [Route("DeleteDish/{dishId}")]
        public async Task<IActionResult> DeleteDish(int dishId)
        {
            var deletedDish = await _marioBusinessLayer.DeleteDishAsync(dishId);

            if (deletedDish == null)
            {
                return NotFound();
            }

            return Ok(deletedDish);
        }

        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(IEnumerable<Dish>))]
        [Route("GetDishesByCourseId/{courseId}")]
        public async Task<IActionResult> GetDishesByCourseId(int courseId)
        {
            var data = await _marioBusinessLayer.GetDishesByCourseIdAsync(courseId);
            return Ok(data);
        }
    }

}
