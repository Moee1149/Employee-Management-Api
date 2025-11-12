namespace MyWebApiApp.Iservice;

public interface IFileService
{
    public Task SaveFileToDisk(IFormFile file);
    public Task GetFileFromDisk();
}

