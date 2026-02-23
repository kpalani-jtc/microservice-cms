namespace ServiceAppointments.Application.Dtos;

public sealed record ServiceAppointmentDto(Guid Id, Guid LeadId, DateTime ScheduledAtUtc, string Technician, string Status, DateTime CreatedAtUtc);
public sealed record CreateServiceAppointmentRequest(Guid LeadId, DateTime ScheduledAtUtc, string Technician);
public sealed record UpdateServiceAppointmentRequest(Guid LeadId, DateTime ScheduledAtUtc, string Technician, string Status);
