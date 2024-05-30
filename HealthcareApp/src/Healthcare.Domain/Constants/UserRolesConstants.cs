namespace Domain.Constants;

public static class UserRolesConstants
{
    public static readonly string[] AllowedRoles = new[] { "Doctor", "Patient" };

    public static bool IsRoleAllowed(string role) => AllowedRoles.Contains(role);
}