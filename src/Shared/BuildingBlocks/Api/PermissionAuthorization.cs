using System.Security.Claims;
using BuildingBlocks.Application;
using Microsoft.AspNetCore.Authorization;

namespace BuildingBlocks.Api;

public sealed record PermissionRequirement(string Permission) : IAuthorizationRequirement;

public sealed class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IPermissionService _permissionService;

    public PermissionHandler(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var subject = context.User.FindFirstValue("oid") ?? context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (subject is null)
            return;

        if (await _permissionService.HasPermissionAsync(subject, requirement.Permission))
            context.Succeed(requirement);
    }
}
