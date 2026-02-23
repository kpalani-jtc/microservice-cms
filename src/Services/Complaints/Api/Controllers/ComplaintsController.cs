using Complaints.Application.Dtos;
using Complaints.Application.Ports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Complaints.Api.Controllers;

[ApiController]
[Route("api/complaints")]
[Authorize]
public class ComplaintsController : ControllerBase
{
    private readonly IComplaintUseCase _useCase;

    public ComplaintsController(IComplaintUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public Task<IReadOnlyCollection<ComplaintDto>> GetAll(CancellationToken cancellationToken)
        => _useCase.GetAllAsync(cancellationToken);

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        => (await _useCase.GetByIdAsync(id, cancellationToken)) is { } dto ? Ok(dto) : NotFound();

    [HttpPost]
    [Authorize(Policy = "CanWrite")]
    public async Task<IActionResult> Create([FromBody] CreateComplaintRequest request, CancellationToken cancellationToken)
    {
        var id = await _useCase.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "CanWrite")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateComplaintRequest request, CancellationToken cancellationToken)
        => await _useCase.UpdateAsync(id, request, cancellationToken) ? NoContent() : NotFound();

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "CanWrite")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        => await _useCase.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}
