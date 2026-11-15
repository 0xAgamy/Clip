
namespace Clips_api.Services;

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream content, string fileName,
                             string contnetType,
                             string folder, CancellationToken ct = default);


    Task DeleteAsync(string ObjectKey, CancellationToken ct = default);

    Task<string> GetPresignedUrlAsync(string ObjectKey, CancellationToken ct = default);
}
