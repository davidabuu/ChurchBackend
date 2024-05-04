using Microsoft.AspNetCore.Mvc;

namespace ChurchWebApp.Controllers
{
    public class ImageController : Controller
    {
        [HttpPost]
        [Route("upload")]
        public async Task<string> WriteFile(IFormFile file)
        {
            string filename = "";
            try
            {
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\Files", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return filename;

            }
            catch (Exception ex)
            {
            }
            return filename;
        }
        [HttpGet]
        [Route("images/{filename}")]
        public IActionResult GetImage(string filename)
        {
            try
            {
                // Define the path to the uploaded files
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Upload", "Files", filename);

                // Check if the file exists
                if (!System.IO.File.Exists(filepath))
                {
                    return NotFound(); // Return 404 if the file is not found
                }

                // Determine the content type (MIME type) based on the file extension
                string contentType = "application/octet-stream"; // Default type if extension is not recognized
                if (filename.EndsWith(".jpg") || filename.EndsWith(".jpeg"))
                {
                    contentType = "image/jpeg";
                }
                else if (filename.EndsWith(".png"))
                {
                    contentType = "image/png";
                }
                // Add more content types as needed (e.g., ".gif" for GIF images)

                // Return the file using the File method with the appropriate content type
                return File(System.IO.File.OpenRead(filepath), contentType);
            }
            catch (Exception ex)
            {
                // Handle any exceptions and return a 500 Internal Server Error
                return StatusCode(500, ex.Message);
            }
        }
    

}
}
