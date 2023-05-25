namespace Bilet1.Utilities.Extensions
{
    public static class MyExtensions
    {
        public static bool CheckContentType(this IFormFile file, string contentType)
        {
            return file.ContentType.ToLower().Trim().Contains(contentType.ToLower().Trim());
        }
        public static bool CheckFileSize(this IFormFile file, double size)
        {
            return file.Length /1024 <size;
        }
        public async static Task<string> SaveAsync(this IFormFile file, string root)
        {
            string fileName = Guid.NewGuid().ToString() + file.FileName;
            string resultPath = Path.Combine(root, fileName);
            using (FileStream fileStream = new FileStream(resultPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }


    }
}
