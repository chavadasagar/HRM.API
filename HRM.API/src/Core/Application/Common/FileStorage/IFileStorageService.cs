namespace HRM.API.Application.Common.FileStorage;

public interface IFileStorageService : ITransientService
{
    public Task<string> UploadAsync<T>(FileUploadRequest? request, FileType supportedFileType, CancellationToken cancellationToken = default)
    where T : class;

    public Task<string> UploadAsync<T>(FileUploadRequest? request, string fileFullPath, FileType supportedFileType, CancellationToken cancellationToken = default)
    where T : class;

    public void Remove(string? path);
}