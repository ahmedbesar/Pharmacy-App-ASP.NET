using Microsoft.AspNetCore.Http;
 

namespace Pharmacy.Services.Settings
{
    public static class UploadImages
    {
        public static async Task<string> UploadImage(IFormFile file,string folderName)
        {
                string ImageName = Guid.NewGuid().ToString() + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + ".jpg";
                    var filePaths = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads/images/"+ folderName, ImageName);
                    using (var stream = System.IO.File.Create(filePaths))
                    {
                        await file.CopyToAsync(stream);
                        return ImageName;
                    }
                

        }
    }
}
