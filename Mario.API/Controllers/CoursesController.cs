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
                Description = x.Description
            }).ToList();

            return Ok(response);
        }

        [HttpGet]
        [ProducesResponseType(statusCode: 200, type: typeof(CourseResponse))]
        [Route("GetCourseById/{courseId}")]
        public async Task<IActionResult> GetCourseById(int courseId)
        {
            var course = await _marioBusinessLayer.GetCourseByIdAsync(courseId);

            if (course == null)
            {
                return NotFound();
            }

            var response = new CourseResponse
            {
                Id = course.Id,
                Description = course.Description
            };

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(statusCode: 201, type: typeof(CourseResponse))]
        [Route("CreateCourse")]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var course = new Course
            {
                Description = request.Description
            };

            await _marioBusinessLayer.AddCourseAsync(course);

            var response = new CourseResponse
            {
                Id = course.Id,
                Description = course.Description
            };

            return CreatedAtAction(nameof(FetchAllCourses), new { id = response.Id }, response);
        }

        [HttpPut]
        [ProducesResponseType(statusCode: 200, type: typeof(CourseResponse))]
        [Route("UpdateCourse")]
        public async Task<IActionResult> UpdateCourse([FromBody] CourseUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingCourse = await _marioBusinessLayer.GetCourseByIdAsync(request.Id);

            if (existingCourse == null)
            {
                return NotFound();
            }

            existingCourse.Description = request.Description;

            await _marioBusinessLayer.UpdateCourseAsync(existingCourse);

            var response = new CourseResponse
            {
                Id = existingCourse.Id,
                Description = existingCourse.Description
            };

            return Ok(response);
        }

        [HttpDelete]
        [ProducesResponseType(statusCode: 200, type: typeof(CourseResponse))]
        [Route("DeleteCourse/{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            var existingCourse = await _marioBusinessLayer.GetCourseByIdAsync(courseId);

            if (existingCourse == null)
            {
                return NotFound();
            }

            var deletedCourse = await _marioBusinessLayer.DeleteCourseAsync(courseId);

            var response = new CourseResponse
            {
                Id = deletedCourse.Id,
                Description = deletedCourse.Description
            };

            return Ok(response);
        }

    }
}
