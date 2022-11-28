
using Course4Me.Logic;
using Course4Me_ServerSide.Data;
using Course4Me_ServerSide.Interfaces;
using Course4Me_ServerSide.Repos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Course4Me_ServerSide.InnerUtils
{

    public class JwtAuthenticationManager :  DbContext, IJwtAuthenticationManager
    {
        private AppDbContext _context;
        private IUtils _utils;
        private readonly string key;
        public JwtAuthenticationManager(AppDbContext context,IUtils utils)
        {
            _utils= utils;
            _context = context;
             
        }
        public JwtAuthenticationManager(string key) 
        {
            this.key=key;
        }
        public string Authenticate(string username, string password)
        {
          
            password = _utils.HashPassword(password);
            if (!_context.Users.Any(u => u.Email == username && u.Password == password))
            {
                return null;
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);


        }
    }
}
