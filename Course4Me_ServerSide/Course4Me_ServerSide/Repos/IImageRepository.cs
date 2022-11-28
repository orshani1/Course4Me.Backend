
namespace Course4Me_ServerSide.Repos
{
    public interface IImageRepository
    {
        Task AddCourseImageAsync(IFormFile file, int courseId);
         Byte[] GetImageFromPcAsync(int id, out string imageName);
        string GetImagePath(int id, out string fileName);
        int AddNewImage(IFormFile file);
    }
}