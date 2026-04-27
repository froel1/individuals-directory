using Microsoft.Extensions.Options;

namespace IndividualsDirectory.Service.Images;

public class FileSystemImageStorageService : IImageStorageService
{
    private readonly string _basePath;

    public FileSystemImageStorageService(IOptions<ImageStorageOptions> options)
    {
        _basePath = Path.GetFullPath(options.Value.BasePath);
    }

    public async Task<Guid> SaveAsync(Stream content, string fileName, CancellationToken ct = default)
    {
        Directory.CreateDirectory(_basePath);

        var imageId = Guid.NewGuid();
        var ext = Path.GetExtension(fileName);
        var path = Path.Combine(_basePath, $"{imageId}{ext}");

        await using var fs = File.Create(path);
        await content.CopyToAsync(fs, ct);

        return imageId;
    }

    public Task DeleteAsync(Guid imageId, CancellationToken ct = default)
    {
        if (!Directory.Exists(_basePath))
        {
            return Task.CompletedTask;
        }

        foreach (var file in Directory.EnumerateFiles(_basePath, $"{imageId}.*"))
        {
            File.Delete(file);
        }

        return Task.CompletedTask;
    }
}
