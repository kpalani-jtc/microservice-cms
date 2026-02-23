namespace Master.Application.Dtos;

public sealed record MasterItemDto(Guid Id, string Category, string Code, string Value, bool IsActive, DateTime CreatedAtUtc);
public sealed record CreateMasterItemRequest(string Category, string Code, string Value);
public sealed record UpdateMasterItemRequest(string Category, string Code, string Value, bool IsActive);
