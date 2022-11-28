using Course4Me_ServerSide.Data;
using Course4Me_ServerSide.Models;
using Microsoft.EntityFrameworkCore;

namespace Course4Me_ServerSide.Repos
{
    public class VideoRepository : IVideoRepository
    {
        private AppDbContext _context;
        private IWebHostEnvironment _env;

        public VideoRepository(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<int> UploadVideo(IFormFile file,int courseId)
        {
            
            var finalProjFolder = _env.ContentRootPath.Split("Backend");
            var pathBeforeAdjustment = finalProjFolder[0];
            var folderNamesWithoutBackSlashes = pathBeforeAdjustment.Split('\\');
            var finalPath = string.Empty;
            
            foreach(var folderName in folderNamesWithoutBackSlashes)
            {
                if (folderName != "" || !String.IsNullOrEmpty(folderName))
                {
                finalPath += $"{folderName}/";
                }
            }
            finalPath += "clientSide/app/src/assets/videos/";

            var filePath = Path.Combine(finalPath, new Guid() +  file.FileName);
            using (var steam = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(steam);
                var vidObj = new VideoObject()
                {
                    FileName = file.FileName,
                    Path = filePath,
                    
                    CourseId = courseId,
                    
                };
                await _context.Videos.AddAsync(vidObj);
                await _context.SaveChangesAsync();

                ////Call Episode Repository?
                
                
                return  vidObj.Id;

            }


            
        }
        public Byte[] GetVideo(int videoId)
        {
            var video = _context.Videos.Find(videoId);   
            Byte[] videoAsByte = File.ReadAllBytes(video.Path);
            return videoAsByte;

        }
        public  string GetVideoPath(int id)
        {
           var vid =  _context.Videos.Find(id);
            return vid.Path;    
            
        }

        public List<string> GetVideosPathByCourseId(int courseId)
        {
            var videos = _context.Videos.Select(video => video).Where(v=>v.CourseId == courseId).ToList();
            List<string> videosPaths = new List<string>();
            foreach(var video in videos)
            {
                var path = video.Path;  
                var pathSplit = path.Split("src");
                video.Path = "../../../.." + pathSplit[1];
                videosPaths.Add(video.Path);    
            }
            return videosPaths;
        }
        public string GetVideoPathByVideoId(int videoId)
        {
            var video = _context.Videos.Find(videoId);
            var path = video.Path;
            var pathSplit = path.Split("src");
            video.Path = "../../../.." + pathSplit[1];
            return video.Path;
        }
        public async Task<int> UploadNewEpisodeAsync(Episode episode)
        {
            await _context.Episodes.AddAsync(episode);
            await _context.SaveChangesAsync();
            return episode.Id;
        }
        public async Task<List<Episode>> GetEpisodesAsync(int courseId)
        {
            return await _context.Episodes.Select(s => s).Where(s => s.CourseId == courseId).ToListAsync();
        }
     }
}
