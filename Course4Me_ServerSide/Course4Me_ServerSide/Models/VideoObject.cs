using System.ComponentModel.DataAnnotations;

namespace Course4Me_ServerSide.Models
{
    public class VideoObject
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        
        public int CourseId { get; set; }


    }
}
