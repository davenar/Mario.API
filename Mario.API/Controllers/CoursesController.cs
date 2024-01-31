using Mario.API.Contracts.Responses;
using Mario.Core.BusinessLayers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mario.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly MarioBusinessLayer _marioBusinessLayer;

        public CoursesController(MarioBusinessLayer marioBusinessLayer)
        {
            _marioBusinessLayer = marioBusinessLayer;
        }


        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(IEnumerable<CourseResponse>))]
        [Route("FetchAllCourses")]
        public async Task<IActionResult> FetchAllCourses()
        {
            var data = await _marioBusinessLayer.GetAllCoursesAsync();
            var response = data.Select(x => new CourseResponse
            {
                Id = x.Id,
                Description = x.Description,
                AvailableDishes = new List<DishResponse>(),
            }).ToList();

            return Ok(response);
        }
    }
}
