
using Course4Me_ServerSide.Models;

namespace Course4Me_ServerSide.Repos
{
    public interface IVideoRepository
    {
        Byte[] GetVideo(int videoId);
        Task<int> UploadVideo(IFormFile file, int courseId);
        string GetVideoPath(int id);
        List<string> GetVideosPathByCourseId(int courseId);
        Task<int> UploadNewEpisodeAsync(Episode episode);
        Task<List<Episode>> GetEpisodesAsync(int courseId);
        string GetVideoPathByVideoId(int videoId);
    }
}