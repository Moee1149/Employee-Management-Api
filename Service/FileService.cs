
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

    public Task GetFileFromDisk()
    {
        throw new NotImplementedException();
    }

    public async Task SaveFileToDisk(IFormFile file)
    {
        var filePath = Path.Combine(_uploadPath, file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
            Console.WriteLine("file writeto disk");
        }
    }
}