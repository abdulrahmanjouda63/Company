namespace Company.PL.Helpers
{
    public static class DocumentSettings
    {
        // 1. Upload File
        // ImageName
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Folder Location
            //string folderPath = "C:\\Users\\gehad eldahshory\\source\\repos\\Company\\Company.PL\\wwwroot\\files\\" + folderName;

            //var folderPath = Directory.GetCurrentDirectory() + "\\wwwroot\\files\\" + folderName;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", folderName);

            // Ensure the directory exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Get File Name And Make It Unique
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            // File Path
            var filePath = Path.Combine(folderPath, fileName);

            // Ensure the file stream is properly closed and disposed of
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return fileName;
        }

        // 2. Delete File

        public static void DeleteFile(string fileName, string folderName)
        {

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", folderName, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

        }

    }
}
