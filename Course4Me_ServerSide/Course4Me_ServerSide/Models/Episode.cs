namespace Course4Me_ServerSide.Models
{
    public class Episode
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int VideoId { get; set; }    
        public string EpisodeName { get; set; }
        public string Description { get; set; }
        public int NumberOfOrder { get; set; }

    }
}
