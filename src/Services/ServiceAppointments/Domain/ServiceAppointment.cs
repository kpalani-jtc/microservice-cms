namespace ServiceAppointments.Domain;

public sealed class ServiceAppointment
{
    public Guid Id { get; set; }
    public Guid LeadId { get; set; } = Guid.Empty;
    public DateTime ScheduledAtUtc { get; set; } = DateTime.UtcNow;
    public string Technician { get; set; } = string.Empty;
    public string Status { get; set; } = "Scheduled";
    public DateTime CreatedAtUtc { get; set; }
}
