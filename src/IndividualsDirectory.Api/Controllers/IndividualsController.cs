using IndividualsDirectory.Service.Models;
using IndividualsDirectory.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace IndividualsDirectory.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IndividualsController(IIndividualService service) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet("quick-search")]
    public async Task<IActionResult> QuickSearch([FromQuery] QuickSearchRequest request, CancellationToken ct)
    {
        var result = await service.QuickSearchAsync(request, ct);
        return Ok(result);
    }

    [HttpGet("detailed-search")]
    public async Task<IActionResult> DetailedSearch([FromQuery] DetailedSearchRequest request, CancellationToken ct)
    {
        var result = await service.DetailedSearchAsync(request, ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateIndividualRequest request, CancellationToken ct)
    {
        var newId = await service.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = newId }, new { id = newId });
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Edit(int id, UpdateIndividualRequest request, CancellationToken ct)
    {
        var updated = await service.UpdateAsync(id, request, ct);
        return updated ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var deleted = await service.DeleteAsync(id, ct);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPost("{id:int}/image")]
    public async Task<IActionResult> UploadImage(int id, IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0)
        {
            return BadRequest("No file provided.");
        }

        await using var stream = file.OpenReadStream();
        var imageId = await service.UploadImageAsync(id, stream, file.FileName, ct);
        return imageId is null ? NotFound() : Ok(new { imageId });
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
