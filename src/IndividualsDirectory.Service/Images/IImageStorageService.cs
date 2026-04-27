namespace IndividualsDirectory.Service.Images;

public interface IImageStorageService
{
    Task<Guid> SaveAsync(Stream content, string fileName, CancellationToken ct = default);
    Task<ImageFile?> GetAsync(Guid imageId, CancellationToken ct = default);
    Task DeleteAsync(Guid imageId, CancellationToken ct = default);
}
