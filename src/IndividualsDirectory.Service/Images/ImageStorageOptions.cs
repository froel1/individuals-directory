namespace IndividualsDirectory.Service.Images;

public class ImageStorageOptions
{
    public string BasePath { get; set; } = "uploads/individuals";
    public long MaxBytes { get; set; } = 5 * 1024 * 1024;
    public string[] AllowedContentTypes { get; set; } =
    [
        "image/jpeg",
        "image/png",
        "image/gif",
        "image/webp",
    ];
}
