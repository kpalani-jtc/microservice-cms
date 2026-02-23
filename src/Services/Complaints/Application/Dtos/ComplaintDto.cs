namespace Complaints.Application.Dtos;

public sealed record ComplaintDto(Guid Id, string Title, string Description, string Status, DateTime CreatedAtUtc);
public sealed record CreateComplaintRequest(string Title, string Description);
public sealed record UpdateComplaintRequest(string Title, string Description, string Status);
