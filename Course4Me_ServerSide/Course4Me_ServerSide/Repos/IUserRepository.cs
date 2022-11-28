using Course4Me_ServerSide.Models;

namespace Course4Me_ServerSide.Repos
{
    public interface IUserRepository
    {

        Task<int> AddUserAsync(User user, IFormFile img);
        string Authenticate(string username, string password);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetSingleUser(int id);
        Task<bool> RemoveUser(int id);
        Task<User?> UpdateUser(User user);
        Task<User?> Login(string username, string password);
        Task<bool> BuyCourse(int courseId, User user);
    }
}