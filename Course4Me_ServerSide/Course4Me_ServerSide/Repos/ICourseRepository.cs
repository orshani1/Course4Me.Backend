using Course4Me_ServerSide.Models;

namespace Course4Me_ServerSide.Repos
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetRelaventCourses(int instructorId);
        Task<string> AddNewRating(int newRatingValue, int courseId);
        Task<int> AddCourseAsync(Course course, User user);
        Task<List<Course>> GetAllCoursesAsync();
        Task<Course?> GetSingleCourse(int id);
        Task<bool> RemoveCourse(int id);
        Task<Course?> UpdateCourse(Course course);
        Task<List<Course?>> GetPurchasedCourses(int userId);
    }
}