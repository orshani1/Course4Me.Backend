using Course4Me_ServerSide.Data;
using Course4Me_ServerSide.Models;
using System.Drawing;

namespace Course4Me_ServerSide.Repos
{
    public class ImageRepository : IImageRepository
    {
        private AppDbContext _context;
        private IWebHostEnvironment _env;
        public ImageRepository(AppDbContext context,IWebHostEnvironment en)
        {
            _env = en;
            _context = context;
        }
        public async Task AddCourseImageAsync(IFormFile file,int courseId)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream); /// copy the file to the memorystream Instace
                   
               
                using (var img = Image.FromStream(memoryStream)) /// Converting memorySteam to Image
                {
                    var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    
                    var path = Path.Combine(_env.ContentRootPath ,"Resources","Images" , new Guid() +  file.Name + ".jpg"); // make full path for image example : desktop/photo1.jpg
                    img.Save(path); // save the image
                    ImageObject imageObject = new ImageObject()
                    {
                        Name = file.FileName,
                        Path = path,
                    };
                    _context.Images.Add(imageObject);
                    await _context.SaveChangesAsync();
                    var course = await _context.Courses.FindAsync(courseId);
                    course.CourseImageId = imageObject.Id;
                    await _context.SaveChangesAsync();  

                    
                    
                }
            }
        }
        public int AddNewImage(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                 file.CopyTo(memoryStream); /// copy the file to the memorystream Instace


                using (var img = Image.FromStream(memoryStream)) /// Converting memorySteam to Image
                {
                    var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

                    var path = Path.Combine(_env.ContentRootPath, "Resources", "Images", new Guid() + file.Name + ".jpg"); // make full path for image example : desktop/photo1.jpg
                    img.Save(path); // save the image
                    ImageObject imageObject = new ImageObject()
                    {
                        Name = file.FileName,
                        Path = path,
                    };
                    _context.Images.Add(imageObject);
                    _context.SaveChanges();
                    return imageObject.Id;



                }
            }
        }
        public  Byte[] GetImageFromPcAsync(int id,out string imageName)
        {
            var image = _context.Images.Find(id);
            Byte[] b =  File.ReadAllBytes(image.Path);
            imageName = image.Name;
            return b;

        }
        public string GetImagePath(int id , out string fileName)
        {
            var img = _context.Images.Find(id);
            fileName = img.Name;
            return img.Path;    
        }
      
    }
}
