using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;

namespace IndividualsDirectory.Service.Images;

public class FileSystemImageStorageService : IImageStorageService
{
    private static readonly FileExtensionContentTypeProvider ContentTypeProvider = new();

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

    public Task<ImageFile?> GetAsync(Guid imageId, CancellationToken ct = default)
    {
        if (!Directory.Exists(_basePath))
        {
            return Task.FromResult<ImageFile?>(null);
        }

        var match = Directory.EnumerateFiles(_basePath, $"{imageId}.*").FirstOrDefault();
        if (match is null)
        {
            return Task.FromResult<ImageFile?>(null);
        }

        if (!ContentTypeProvider.TryGetContentType(match, out var contentType))
        {
            contentType = "application/octet-stream";
        }

        Stream stream = File.OpenRead(match);
        return Task.FromResult<ImageFile?>(new ImageFile(stream, contentType));
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
