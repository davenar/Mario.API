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
                Course = x?.Course is not null ? new CourseResponse { Id = x.Course.Id, Description = x?.Course?.Description } : null,
                Image = x.Image,
                Ingredients = x?.Ingredients ?? new List<string>(),
                Name = x.Name,
                Price = x.Price
            }).ToList();

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(IEnumerable<DishResponse>))]
        [Route("GetDishesByCourseId/{courseId}")]
        public async Task<IActionResult> GetDishesByCourseId(int courseId)
        {
            var data = await _marioBusinessLayer.GetDishesByCourseIdAsync(courseId);

            var response = data.Select(x => new DishResponse
            {
                Id = x.Id,
                Course = x?.Course is not null ? new CourseResponse { Id = x.Course.Id, Description = x?.Course?.Description } : null,
                Image = x.Image,
                Ingredients = x?.Ingredients ?? new List<string>(),
                Name = x.Name,
                Price = x.Price
            }).ToList();

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(DishResponse))]
        [Route("GetDishById/{dishId}")]
        public async Task<IActionResult> GetDishById(int dishId)
        {
            var dish = await _marioBusinessLayer.GetDishByIdAsync(dishId);

            if (dish == null)
            {
                return NotFound();
            }

            var response = new DishResponse
            {
                Id = dish.Id,
                Course = dish?.Course is not null ? new CourseResponse { Id = dish.Course.Id, Description = dish?.Course?.Description } : null,
                Image = dish?.Image,
                Ingredients = dish?.Ingredients ?? new List<string>(),
                Name = dish?.Name,
                Price = dish?.Price
            };

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(statusCode: 201, type: typeof(DishResponse))]
        [Route("CreateDish")]
        public async Task<IActionResult> CreateDish([FromForm] DishCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the specified CourseId exists
            if (request.CourseId.HasValue)
            {
                var course = await _marioBusinessLayer.GetCourseByIdAsync(request.CourseId.Value);

                if (course == null)
                {
                    return BadRequest($"Course with Id {request.CourseId} does not exist.");
                }
            }

            // Check file size
            if (request.Image != null)
            {
                if (request.Image.Length > 2 * 1024 * 1024) // 2MB
                {
                    return BadRequest("Image size exceeds the limit of 2MB.");
                }
            }

            var dish = new Dish
            {
                Name = request.Name,
                Image = await GetBytesFromFormFile(request.Image),
                Ingredients = request.Ingredients ?? new List<string>(),
                Price = request.Price,
                CourseId = request.CourseId
            };

            await _marioBusinessLayer.AddDishAsync(dish);

            var response = new DishResponse
            {
                Id = dish.Id,
                Course = dish?.Course is not null ? new CourseResponse { Id = dish.Course.Id, Description = dish?.Course?.Description } : null,
                Image = dish?.Image,
                Ingredients = dish?.Ingredients ?? new List<string>(),
                Name = dish?.Name,
                Price = dish?.Price
            };

            return CreatedAtAction(nameof(FetchAllDishes), new { id = dish.Id }, response);
        }

        private async Task<byte[]> GetBytesFromFormFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        [HttpPut]
        [ProducesResponseType(statusCode: 200, type: typeof(DishResponse))]
        [Route("UpdateDish")]
        public async Task<IActionResult> UpdateDish([FromForm] DishUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if the specified CourseId exists
            if (request.CourseId.HasValue)
            {
                var course = await _marioBusinessLayer.GetCourseByIdAsync(request.CourseId.Value);

                if (course == null)
                {
                    return BadRequest($"Course with Id {request.CourseId} does not exist.");
                }
            }

            var existingDish = await _marioBusinessLayer.GetDishByIdAsync(request.Id);

            if (existingDish == null)
            {
                return NotFound();
            }

            // Check file size
            if (request.Image != null)
            {
                if (request.Image.Length > 2 * 1024 * 1024) // 2MB
                {
                    return BadRequest("Image size exceeds the limit of 2MB.");
                }
            }

            existingDish.Name = request.Name;
            existingDish.Image = await GetBytesFromFormFile(request.Image);
            existingDish.Ingredients = request.Ingredients ?? new List<string>();
            existingDish.Price = request.Price;
            existingDish.CourseId = request.CourseId;

            await _marioBusinessLayer.UpdateDishAsync(existingDish);

            var response = new DishResponse
            {
                Id = existingDish.Id,
                Course = existingDish?.Course is not null ? new CourseResponse { Id = existingDish.Course.Id, Description = existingDish?.Course?.Description } : null,
                Image = existingDish?.Image,
                Ingredients = existingDish?.Ingredients ?? new List<string>(),
                Name = existingDish?.Name,
                Price = existingDish?.Price
            };

            return Ok(response);
        }

        [HttpDelete]
        [ProducesResponseType(statusCode: 200, type: typeof(DishResponse))]
        [Route("DeleteDish/{dishId}")]
        public async Task<IActionResult> DeleteDish(int dishId)
        {
            var deletedDish = await _marioBusinessLayer.DeleteDishAsync(dishId);

            if (deletedDish == null)
            {
                return NotFound();
            }

            var response = new DishResponse
            {
                Id = deletedDish.Id,
                Course = deletedDish?.Course is not null ? new CourseResponse { Id = deletedDish.Course.Id, Description = deletedDish?.Course?.Description } : null,
                Image = deletedDish.Image,
                Ingredients = deletedDish?.Ingredients ?? new List<string>(),
                Name = deletedDish.Name,
                Price = deletedDish.Price
            };

            return Ok(response);
        }






    }

}
