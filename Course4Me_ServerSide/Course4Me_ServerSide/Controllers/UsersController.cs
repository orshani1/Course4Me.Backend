using Course4Me.Logic;
using Course4Me_ServerSide.Data;
using Course4Me_ServerSide.Interfaces;
using Course4Me_ServerSide.Models;
using Course4Me_ServerSide.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Course4Me_ServerSide.Controllers
{   
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepository _userRepository;
        private IUtils _utils;
        private IJwtAuthenticationManager _authManager;
        public UsersController(IUserRepository repo,IUtils utils,IJwtAuthenticationManager auth)
        {
        _utils = utils; 
        _userRepository = repo;
        _authManager = auth;    
        }
        [HttpGet]

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        [HttpGet("{id}")]
        public async Task<User?> GetSingleUserAsync([FromRoute] int id)
        {
             var user = await _userRepository.GetSingleUser(id);
            
                return user;
        }
        [HttpGet("login")]
        public async Task<User?> LoginAsync([FromQuery] string username,[FromQuery] string password)
        {
            password = _utils.HashPassword(password);
            var user = await _userRepository.Login(username, password);
            return user;
        }
        [AllowAnonymous]
        [HttpPost("add-user")]
        public async Task<ActionResult<int>> AddSingleUser([FromQuery] string email,[FromQuery] string password)
        {
            Regex r = new Regex(@"[a-z0-9]+@[a-z]+\.[a-z]{2,3}");
            Regex r2 = new Regex(@"^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8}$");
            if (!r.IsMatch(email) || !r2.IsMatch(password))
            {
                return BadRequest();
            }
   
           
            User u = new User()
            {
                Password = _utils.HashPassword(password),
                Email = email
            };
            var img = Request.Form.Files.FirstOrDefault();
            var addedUserId = await _userRepository.AddUserAsync(u,img);
            return addedUserId;
        }

        [HttpDelete("delete/{id}")]
        public async Task<bool> DeleteUserAsync( int id)
        {
            var isDeleted = await _userRepository.RemoveUser(id);
            return isDeleted;
        }

        [HttpPut("update")]
        public async Task<User?> UpdateUserAsync([FromBody] User userToUpdate)
        {
            var updatedUser = await _userRepository.UpdateUser(userToUpdate);
            return updatedUser;
        }

        [HttpGet("buy-course/{courseId}")]
        public async Task<bool> BuyCourse([FromRoute] int courseId, [FromQuery] int userId)
        {
            var user = await _userRepository.GetSingleUser(userId);
            var result = await _userRepository.BuyCourse(courseId, user);
            return result;
            
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] User user)
        {
            var token = _userRepository.Authenticate(user.Email, user.Password);
          
            if(token == null)
            {
                return Unauthorized();
            }
            var json =  JsonSerializer.Serialize(token);
            return Ok(json);
        }

        
    }
}
