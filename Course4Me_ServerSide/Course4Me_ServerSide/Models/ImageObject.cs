using System.ComponentModel.DataAnnotations;

namespace Course4Me_ServerSide.Models
{
    public class ImageObject
    {
        [Key]
        public int Id { get; set; } 
        public string Name { get; set; } 
        public string Path { get; set; }    
    }
}
