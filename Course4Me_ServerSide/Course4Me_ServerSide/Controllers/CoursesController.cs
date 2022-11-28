using Course4Me_ServerSide.Models;
using Course4Me_ServerSide.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Course4Me_ServerSide.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private ICourseRepository _courseRepository;
        private IUserRepository _userRepository;
        private IImageRepository _imageRepository;
        private IVideoRepository _videoRepository;
        private IWebHostEnvironment _env;

        public CoursesController(ICourseRepository repo,IUserRepository usRepo,IImageRepository imgRepo,IVideoRepository vidRepo,IWebHostEnvironment env)
        {
            _courseRepository = repo;
            _userRepository = usRepo;
            _imageRepository = imgRepo;
            _videoRepository = vidRepo;
            _env = env;
            
            
        }
        [HttpGet]

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _courseRepository.GetAllCoursesAsync();
        }

        [HttpGet("{id}")]
        public async Task<Course?> GetSingleCourseAsync( int id)
        {
            var user = await _courseRepository.GetSingleCourse(id);

            return user;
        }

        [HttpPost("add-course")]
        public async Task<int> AddSingleCourse([FromBody] Course course,[FromQuery] int userId)
        {
            
            var user = await _userRepository.GetSingleUser(userId); 
            var addedCourseId = await _courseRepository.AddCourseAsync(course,user);
            return addedCourseId;
        }
        [HttpPost("add-rating")]
        public async Task<bool> AddRating([FromQuery]int courseId,[FromQuery]int rating)
        {
            var msg = await _courseRepository.AddNewRating(rating,courseId);
            return true;
        }
        [HttpDelete("delete/{id}")]
        public async Task<bool> DeleteUserAsync(int id)
        {
            var isDeleted = await _courseRepository.RemoveCourse(id);
            return isDeleted;
        }

        [HttpPut("update")]
        public async Task<Course?> UpdateUserAsync([FromBody] Course courseToUpdate)
        {
            var updatedUser = await _courseRepository.UpdateCourse(courseToUpdate);
            return updatedUser;
        }
        [HttpGet("relavent")]
        public async Task<List<Course>> GetRelaventAsync([FromQuery]int instructorId)
        {

            var courses = await _courseRepository.GetRelaventCourses(instructorId);
            return courses;
        }

        [HttpGet("my-courses/{userId}")]
        public async Task<List<Course>> GetMyCourses(int userId)
        {
            var courses = await _courseRepository.GetPurchasedCourses(userId);
            return courses;
        }
        [HttpPost("image/upload")]
        public async Task<IActionResult> UploadCourseImage([FromQuery]int courseId)
        {
            IFormFile file = Request.Form.Files.FirstOrDefault(); // get the file from the http request
            if(file != null)
            {
                await _imageRepository.AddCourseImageAsync(file,courseId);
                
                return Ok();
            }

            return BadRequest();
        }
        
       
      
        [HttpGet("image/get/{id}")]
        [Produces("image/png")]
        public IActionResult GetImage(int id)
        {

            string fileName;
            string localFilePath;
            localFilePath = _imageRepository.GetImagePath(id,out fileName);
            var image = System.IO.File.OpenRead(localFilePath);
            return File(image, "image/jpeg");

        }

        [HttpGet("video/get/{id}")]
        public IActionResult GetSingleVideoByVideoId(int id)
        {
            var path = _videoRepository.GetVideoPath(id);
            var file = System.IO.File.OpenRead(path);
            return File(file, "video/mp4");
            
            
        }
        [HttpGet("video/get/videos-paths")]
        public IActionResult GetCoursesVideosPaths([FromQuery]int courseId)
        {
            var paths = _videoRepository.GetVideosPathByCourseId(courseId);
           return Ok(paths);
      
        }
        [HttpPost("video/upload")]
        public async Task<IActionResult> UploadVideo([FromQuery] int courseId)
        {
            IFormFile file = Request.Form.Files.FirstOrDefault(); // get the file from the http request
            if(file!= null)
            {
            var res = await _videoRepository.UploadVideo(file,courseId);
            return Ok(res); 
            }

            return BadRequest();
        }


        [HttpPost("video/upload/episode")]
        public async Task<int> UploadNewEpisode(Episode episode)
        {
            await _videoRepository.UploadNewEpisodeAsync(episode);  
            return episode.Id;
        }
        [HttpGet("video/get/episodes")]
        public async Task<List<Episode>> GetEpisodesAsync([FromQuery]int courseId)
        {
            return await _videoRepository.GetEpisodesAsync(courseId);
        }
         
        [HttpGet("video/single/get-path")]
        public string GetVideoPathById([FromQuery]int videoId)
        {
            return  JsonSerializer.Serialize(_videoRepository.GetVideoPathByVideoId(videoId));
        }



    }
}
