using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using TestToken.Repositories.Interfaces;

namespace TestToken.Repositories.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;
        public CloudinaryService(Cloudinary cloudinary, ILogger<CloudinaryService> logger)
        {
            _cloudinary = cloudinary;
            _logger = logger;
        }
        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentNullException("No file provided");

            var allowedTypes = new List<string> { "image/jpeg", "image/png", "image/gif" };
            if (!allowedTypes.Contains(file.ContentType))
                throw new Exception("Unsupported file type. Only JPEG, PNG, and GIF are allowed.");

          //  using to dispose file stream automatically after use
             using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName,stream),
                Transformation = new Transformation().Width(500).Height(500).Crop("fill")
            };
            
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);
            if(uploadResult.Error != null)
                throw new Exception($"Cloudinary upload failed :{uploadResult.Error.Message}");
            _logger.LogInformation($"File uploaded successfully. Public URL: {uploadResult.SecureUrl}");

            return uploadResult.SecureUrl.AbsoluteUri;
        }
    }
}
