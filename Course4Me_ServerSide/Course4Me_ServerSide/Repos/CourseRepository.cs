using Course4Me_ServerSide.Data;
using Course4Me_ServerSide.Models;
using Microsoft.EntityFrameworkCore;

namespace Course4Me_ServerSide.Repos
{
    public class CourseRepository : ICourseRepository
    {
        /// <summary>
        /// Course Rating Structr
        /// 
        /// 5 star - 252
//4 star - 124
//3 star - 40
//2 star - 29
//1 star - 33
        /// </summary>
        //

        //Formula to calculate rating : ((RatingCount * Rating) + new Rating) / (Rating + 1)
        //
        private AppDbContext _context;
        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }
        //READ
        public async Task<List<Course>> GetAllCoursesAsync()
        {
            var course = await _context.Courses.Select(a => a).ToListAsync();
            return course;
        }
        //READSINGLE
        public async Task<Course?> GetSingleCourse(int id)
        {
           
            var course = await _context.Courses.Select(a => a).Where(a => a.Id == id).FirstOrDefaultAsync();
            if (course != null)
            {

                return course;
            }
            return null;
        }


        //CREATE
        public async Task<int> AddCourseAsync(Course course,User user)
        {
            course.AuthorId =user.Id;
            
            
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course.Id;
        }

        //ADD NEW Student
        public async Task<bool> AddNewStudentToCourse(int courseId, User user)
        {
            var course = await _context.Courses.Select(s => s).Where(s => s.Id == courseId).FirstOrDefaultAsync();
            if (course != null)
            {
                course.StudentsId += $"{user.Id},";
                course.NumOfStudents += 1;
                await _context.SaveChangesAsync();
                return true;

            }
            return false;   
        }

        //ADD NEW RATING 
        public async Task<string> AddNewRating(int newRatingValue,int courseId)
        {

            newRatingValue = newRatingValue == 5 ? 252 : newRatingValue == 4 ? 124 : newRatingValue == 3 ? 40 : newRatingValue == 2 ? 33 : 29;
            var course = await _context.Courses.Select(s => s).Where(s => s.Id == courseId).FirstOrDefaultAsync();
            if(course != null)
            {
                var overAllRating = course.Rating;
                var totalRating = course.RatingCount;
                var newRating = ((overAllRating * totalRating) + newRatingValue) / (totalRating + 1);
                course.Rating = newRating;
                course.RatingCount ++;
                await _context.SaveChangesAsync();
                return $"Rating executed successfully old rating is {overAllRating} New Rating is {course.Rating}";

            }
            return "false";
        } 

        //DELETE
        public async Task<bool> RemoveCourse(int id)
        {
            var course = await _context.Courses.Select(a => a).Where(a => a.Id == id).FirstOrDefaultAsync();
            if (course != null)
            {
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;



        }

        //UPDATE
        public async Task<Course?> UpdateCourse(Course course)
        {


            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return course;
        }


        ///Get the courses which the user is instructor
        public async Task<List<Course>> GetRelaventCourses(int instructorId)
        {
   
            var relaventCourses = await _context.Courses.Select(c=>c).Where(s=>s.AuthorId == instructorId).ToListAsync();
            if (relaventCourses != null)
            {
                return relaventCourses;
            }
            return null;
        }

        public async Task<List<Course?>> GetPurchasedCourses(int userId)
        {
            
            var courseList = new List<Course>();
            var courses = await _context.Courses.Select(c => c).ToListAsync();
            foreach(var course in courses)
            {
                var studentsIds = course.StudentsId.Split(",");
                foreach(var id in studentsIds)
                {
                    if(id == Convert.ToString(userId))
                    {
                        courseList.Add(course);
                    }
                }
            }
            return courseList;
        }

        
    }
}
