namespace Lead.Application.Dtos;

public sealed record LeadDto(Guid Id, string Name, string Email, string Phone, string Status, DateTime CreatedAtUtc);
public sealed record CreateLeadRequest(string Name, string Email, string Phone);
public sealed record UpdateLeadRequest(string Name, string Email, string Phone, string Status);
