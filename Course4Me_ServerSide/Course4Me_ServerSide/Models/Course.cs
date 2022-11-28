using System.ComponentModel.DataAnnotations.Schema;

namespace Course4Me_ServerSide.Models
{
    public enum Category
    {
        Development = 0,
        Design = 1,
        Marketing = 2,  
        LifeStyle= 3,   
        HealthAndFitness = 4,
        Music = 5,
    }
    public enum Langauge
    {
        English = 0,
        Hebrew = 1,
        Spanish = 2,

    }
   
    public class Course
    {
      
        public int Id { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        
        public int AuthorId { get; set; }
        
        
        public int Price { get; set; }
        public int Rating { get; set; }
        public int RatingCount { get; set; } 
        public Category Category { get; set; }
        public int NumOfStudents { get; set; }
     
        public string StudentsId { get; set; } 
        public Langauge  Langauge { get; set; }
        public int CourseImageId { get; set; }



    }
}
