using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServiceAppointments.Api.Controllers;

[ApiController]
[Route("api/serviceappointments/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Get() => Ok(new { service = "ServiceAppointments", status = "ok" });
}
