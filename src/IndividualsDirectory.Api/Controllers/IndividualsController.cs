using IndividualsDirectory.Api.Models;
using IndividualsDirectory.Service.Images;
using IndividualsDirectory.Service.Models;
using IndividualsDirectory.Service.Models.Shared;
using IndividualsDirectory.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace IndividualsDirectory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IndividualsController(
    IIndividualService service,
    IImageStorageService imageStorage) : ControllerBase
{
    /// <summary>
    /// Returns the full information about an individual by id, including their phone numbers, connected individuals, and image URL.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        if (result is null) return NotFound();

        var withUrl = result with
        {
            ImageUrl = result.ImageId.HasValue
                ? Url.Action(nameof(GetImage), "Individuals", new { id }, Request.Scheme)
                : null,
        };
        return Ok(withUrl);
    }

    /// <summary>
    /// Quick search by first name, last name, or personal number (any of the fields). Uses SQL LIKE matching with paging.
    /// </summary>
    [HttpGet("quick-search")]
    public async Task<IActionResult> QuickSearch([FromQuery] QuickSearchRequest request, CancellationToken ct)
    {
        var result = await service.QuickSearchAsync(request, ct);
        return Ok(result);
    }

    /// <summary>
    /// Detailed search by every searchable field (exact match) combined with AND, with paging.
    /// </summary>
    [HttpGet("detailed-search")]
    public async Task<IActionResult> DetailedSearch([FromQuery] DetailedSearchRequest request, CancellationToken ct)
    {
        var result = await service.DetailedSearchAsync(request, ct);
        return Ok(result);
    }

    /// <summary>
    /// Adds a new individual along with their phone numbers and (optionally) connected individuals.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateIndividualRequest request, CancellationToken ct)
    {
        var newId = await service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId });
    }

    /// <summary>
    /// Edits an individual's basic information: first name, last name, gender, personal number, date of birth, city, and phone numbers. Only fields present in the request are changed.
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Edit(int id, UpdateIndividualRequest request, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, request, ct);
        return updated ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes an individual along with their owned phone numbers and connections (in either direction).
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await service.DeleteAsync(id, ct);
        return deleted ? NoContent() : NotFound();
    }

    /// <summary>
    /// Uploads or replaces the individual's profile image. The file is stored on the file system and the individual is updated to reference it.
    /// </summary>
    [HttpPost("{id:int}/image")]
    public async Task<IActionResult> UploadImage(int id, [FromForm] UploadImageRequest request, CancellationToken ct)
    {
        await using var stream = request.File.OpenReadStream();
        var imageId = await service.UploadImageAsync(id, stream, request.File.FileName, ct);
        return imageId is null ? NotFound() : Ok(new { imageId });
    }

    /// <summary>
    /// Streams the individual's profile image with the appropriate content type.
    /// </summary>
    [HttpGet("{id:int}/image", Name = nameof(GetImage))]
    public async Task<IActionResult> GetImage(int id, CancellationToken ct)
    {
        var details = await service.GetByIdAsync(id, ct);
        if (details is null || !details.ImageId.HasValue)
        {
            return NotFound();
        }

        var image = await imageStorage.GetAsync(details.ImageId.Value, ct);
        if (image is null)
        {
            return NotFound();
        }

        return File(image.Content, image.ContentType);
    }

    [HttpPut("{id:int}/connections")]
    public async Task<IActionResult> UpdateConnections(int id, List<ConnectedIndividual> connections, CancellationToken ct)
    {
        var updated = await service.UpdateConnectionsAsync(id, connections, ct);
        return updated ? NoContent() : NotFound();
    }
    
    [HttpGet("{id:int}/connections/grouped")]
    public async Task<IActionResult> GetConnectionsGrouped(int id, CancellationToken ct)
    {
        var result = await service.GetConnectionsGroupedAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }
}
