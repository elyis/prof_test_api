namespace prof_tester_api.src.Utility
{
    public class FileUploader
    {
        public static async Task<string?> UploadFileAsync(string directoryPath, Stream stream, string fileExtension)
        {
            if (stream == null || string.IsNullOrWhiteSpace(fileExtension))
                return null;

            string filename = Guid.NewGuid().ToString() + fileExtension;
            string fullPathToFile = Path.Combine(directoryPath, filename);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using var file = File.Create(fullPathToFile);
            await stream.CopyToAsync(file);
            return filename;
        }

        public static async Task<byte[]?> GetStreamFileAsync(string directoryPath, string filename)
        {
            string fullPathToFile = Path.Combine(directoryPath, filename);
            if (!File.Exists(fullPathToFile))
                return null;

            using Stream fileStream = File.OpenRead(fullPathToFile);
            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}