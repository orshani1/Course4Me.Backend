using Course4Me.Logic;
using Course4Me_ServerSide.Data;
using Course4Me_ServerSide.Models;
using Course4Me_ServerSide.Repos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Drawing;


namespace Course4Me_ServerSide.Config
{
    public static class ConfigActions
    {

        private static string _connectionString { get; set; }
        private static IUtils _utils;
        private static SqlConnection _con;
        private static SqlCommand _cmd;
        private static string _dBNameUseCommand = "USE Course4Me;";
        private static List<Course> _courses;
      


        public static void Run(WebApplicationBuilder builder,IWebHostEnvironment env)
        {
            
            _connectionString = builder.Configuration.GetConnectionString("Dev");
            _utils = new Utils();
            _con = new SqlConnection(_connectionString);
            _cmd = new SqlCommand("sp_insert", _con);
            _courses = FillCourseList();
            
            ReadUsers();
            ReadCourses();
            ReadImages(env);
        }
        private static List<Course> FillCourseList()
        {
            return new List<Course>()
            {
                new Course()
                {
                    AuthorId = 1,
                    Category = Category.Marketing,
                    CourseImageId = 1,  
                    CourseName = "Learn Stock marketing",
                    RatingCount = 5,
                    Rating = 252,
                    Description = "Learn how to trade stocks on the stocks market",
                    Langauge = Langauge.English,
                    NumOfStudents = 25,
                    Price = 350,
                    StudentsId = "1,2,3,5,"
                },
                new Course()
                {
                    AuthorId = 1,
                    Category = Category.Design,
                    CourseImageId = 2,
                    CourseName = "Photoshop for begginers",
                    RatingCount = 0,
                    Rating = 0,
                    Description = "Learn how to use photoshop software",
                    Langauge = Langauge.English,
                    NumOfStudents = 1,
                    Price = 350,
                    StudentsId = "1,"
                },
                new Course()
                {
                    AuthorId = 1,
                    CourseImageId = 3,
                    Category = Category.HealthAndFitness,
                    CourseName = "Develope Abs in 5 weeks",
                    RatingCount = 22,
                    Rating = 150,
                    Description = "Let me help you help yourself ! ",
                    Langauge = Langauge.Hebrew,
                    NumOfStudents = 5,
                    Price = 350,
                    StudentsId = "1,2,3,"
                },
                new Course()
                {
                    AuthorId = 2,
                    CourseImageId = 4,
                    Category = Category.HealthAndFitness,
                    CourseName = "Develope Abs in 5 weeks",
                    RatingCount = 22,
                    Rating = 200,
                    Description = "Let me help you help yourself ! ",
                    Langauge = Langauge.Hebrew,
                    NumOfStudents = 5,
                    Price = 350,
                    StudentsId = "1,2,3,"
                },
                new Course()
                {
                    AuthorId = 1,
                    Category = Category.Music,
                    CourseName = "Abelton for begginers",
                    RatingCount = 22,
                    CourseImageId = 5,
                    Rating = 150,
                    Description = "Learn the Software Called Ableton",
                    Langauge = Langauge.Hebrew,
                    NumOfStudents = 5,
                    Price = 350,
                    StudentsId = "1,2,3,"
                }
            };
        }
        private static void ReadUsers()
        {


            _cmd.CommandType = CommandType.Text;
            _cmd.CommandText = $"{_dBNameUseCommand} SELECT * FROM Users";
            _cmd.Connection = _con;
            _con.Open();
            _cmd.ExecuteNonQuery();
            SqlDataReader reader = _cmd.ExecuteReader();
            var count = 0;
            while (reader.Read())
            {
                count++;
            }
            if (count == 0)
            {
                _con.Close();
                SeedUserData();
            }
            reader.Close();
            _con.Close();
            
        }
        private static void ReadCourses()
        {

            _cmd.CommandType = CommandType.Text;
            _cmd.CommandText = $"{_dBNameUseCommand} SELECT * FROM Courses";
            _cmd.Connection = _con;
            _con.Open();
            _cmd.ExecuteNonQuery();
            SqlDataReader reader = _cmd.ExecuteReader();
            var count = 0;
            while (reader.Read())
            {
                count++;
            }
            if (count == 0)
            {
                _con.Close();
                SeedCourseData();
            }
            reader.Close();
            _con.Close();
           

        }
        private static void ReadImages(IWebHostEnvironment env)
        {
            _cmd.CommandType = CommandType.Text;
            _cmd.CommandText = $"{_dBNameUseCommand} SELECT * FROM Images";
            _cmd.Connection = _con;
            _con.Open();
            _cmd.ExecuteNonQuery();
            SqlDataReader reader = _cmd.ExecuteReader();
            var count = 0; 
            while (reader.Read())
            {
                count++;
            }
            if (count == 0)
            {
                _con.Close();
                SeedImages(env);
            }
            reader.Close();
            _con.Close();

        }
        private static void SeedUserData()
        {
            ReseedEntity("Users");
            for (int i = 0; i < 5; i++)
            {

                var userNameVal = "User" + Convert.ToString(i);
                var passwordVal = _utils.HashPassword(Convert.ToString(i));
                _cmd.CommandType = CommandType.Text;
                _cmd.CommandText = $"{_dBNameUseCommand} INSERT Users(Email,Password) VALUES('{userNameVal}','{passwordVal}')";
                _cmd.Connection = _con;
                _con.Open();
                _cmd.ExecuteNonQuery();
                _con.Close();
            }
        }
        private static void SeedCourseData()
        {
            ReseedEntity("Courses");
            for (int i = 0; i < 5; i++)
            {
                var authorId = _courses[i].AuthorId;
                var courseNameVal = _courses[i].CourseName;
                var category =0;
                var language = 0;
                var desc = _courses[i].Description;
                var studentsCount = _courses[i].NumOfStudents;
                var studentsIds = _courses[i].StudentsId;
                var rateCount = _courses[i].RatingCount;
                var rateVal = _courses[i].Rating;
                var price = _courses[i].Price;
                var imageId = _courses[i].CourseImageId;
                _cmd.CommandType = CommandType.Text;
                _cmd.CommandText = $"{_dBNameUseCommand} INSERT Courses" +
                    $"(AuthorId,CourseName,Category,Langauge,Description,NumOfStudents,StudentsId,RatingCount,Rating,Price,CourseImageId)" +
                    $" VALUES('{authorId}','{courseNameVal}','{category}','{language}','{desc}','{studentsCount}','{studentsIds}','{rateCount}','{rateVal}','{price}','{imageId}')";
                _cmd.Connection = _con;
                _con.Open();
                _cmd.ExecuteNonQuery();
                _con.Close();


            }
        }
        private  static void SeedImages(IWebHostEnvironment env)
        {
            ReseedEntity("Images");
            using (var memoryStream = new MemoryStream())
            {
                for(int i = 0;i < 5; i++)
                {
                    var imagesPath = Path.Combine(env.ContentRootPath, "Resources", "Images", $"{i + 1}.jpg");
                    var img = Image.FromFile(imagesPath);
                    ImageObject imageObject = new ImageObject()
                    {
                        Name =  $"{i}",
                        Path = imagesPath,
                    };
                    _cmd.CommandType = CommandType.Text;
                    _cmd.CommandText = $"{_dBNameUseCommand} INSERT Images (Name,Path) VALUES('{imageObject.Name}','{imageObject.Path}')";
                    _cmd.Connection = _con;
                    _con.Open();
                    _cmd.ExecuteNonQuery();
                    _con.Close();

                }
                  
                
            }
        }


        private static void ReseedEntity(string EntityName)
        {
                string query = $"DBCC CHECKIDENT('[{EntityName}]', RESEED, 0);";
                _cmd.CommandType = CommandType.Text;
                _cmd.CommandText = query;
                _cmd.Connection = _con;
                _con.Open();
                _cmd.ExecuteNonQuery();
                _con.Close();
              
        }

        

    }
}
