using MyWebApiApp.Iservice;

namespace MyWebApiApp.Service;

public class FileService : IFileService
{
    private readonly string _uploadPath;

    public FileService()
    {
        _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");
        Directory.CreateDirectory(_uploadPath);
    }

    public List<string> GetAllFiles()
    {
        var fileNames = Directory.GetFiles(_uploadPath).Select(Path.GetFileName).Where(name => name != null).Cast<string>().ToList();
        return fileNames;
    }

    public async Task<byte[]> GetFileFromDisk(string fileName)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileName);

        if (!System.IO.File.Exists(path))
            throw new Exception("File doesn't exits");

        var bytes = await System.IO.File.ReadAllBytesAsync(path);
        return bytes;
    }

    public async Task<string> SaveFileToDisk(IFormFile file)
    {
        var filePath = Path.Combine(_uploadPath, file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
            Console.WriteLine("file writeto disk");
        }
        return filePath;
    }
}