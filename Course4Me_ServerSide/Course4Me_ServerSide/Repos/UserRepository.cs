using Course4Me_ServerSide.Data;
using Course4Me_ServerSide.Models;
using Microsoft.EntityFrameworkCore;
using Course4Me.Logic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Course4Me_ServerSide.Config;

namespace Course4Me_ServerSide.Repos
{
    public class UserRepository : IUserRepository
    {
        private AppDbContext _context;
        private IUtils _utils;
        private IConfiguration _service;
        private IImageRepository _imgRepo;

        public UserRepository(AppDbContext context, IUtils ut, IConfiguration services,IImageRepository imgRepo)
        {
            _utils = ut;
            _context = context;
            _service = services;
            _imgRepo = imgRepo;
            
        }
        //READ
        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _context.Users.Select(a => a).ToListAsync();
            return users;
        }
        //READSINGLE
        public async Task<User?> GetSingleUser(int id)
        {
            var user = await _context.Users.Select(a => a).Where(a => a.Id == id).FirstOrDefaultAsync();
            if (user != null)
            {

                return user;
            }
            return null;
        } 

        //CREATE
        public async Task<int> AddUserAsync(User user,IFormFile img)
        {
          
           var imgId = _imgRepo.AddNewImage(img);
           user.ProfileImageId = imgId;
           _context.Users.Add(user);
           await _context.SaveChangesAsync();
           return user.Id;
        }

        //DELETE
        public async Task<bool> RemoveUser(int id)
        {
            var user = await _context.Users.Select(a => a).Where(a => a.Id == id).FirstOrDefaultAsync();
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;



        }

        //UPDATE
        public async Task<User?> UpdateUser(User user)
        {


            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User?> Login(string username, string password)
        {
            var user = await _context.Users.Select(u => u).Where(u => u.Password == password && u.Email == username).FirstOrDefaultAsync();
            if (user != null)
                return user;
            return null;
        }

        public async Task<bool> BuyCourse(int courseId, User user)
        {
            var course = _context.Courses.Select(c => c).Where(c => c.Id == courseId).FirstOrDefault();
            if (course != null)
            {
                course.StudentsId += $"{user.Id},";
               
                course.NumOfStudents += 1;
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }
        public string Authenticate(string username, string password)
        {

            password = _utils.HashPassword(password);
            if (!_context.Users.Any(u => u.Email == username && u.Password == password))
            {
                return null;
            }
            var claims = new[]
            {
                        new Claim(JwtRegisteredClaimNames.Sub, _service["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_service["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _service["Jwt:Issuer"],
                _service["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        
  
    }
}

