

namespace PaymentApi.Utilities
{
    public static class FileHelper
    {
        private const string UploadFolderName = "uploads";

        public static async Task<string> SaveProfilePicture(IFormFile file, string webRootPath)
        {
            if (file == null || file.Length == 0)
            {
                return string.Empty;
            }

            var uploadsFolder = Path.Combine(webRootPath, UploadFolderName, "profiles");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Return relative path for database storage
            return Path.Combine(UploadFolderName, "profiles", uniqueFileName).Replace("\\", "/");
        }
    }
}
