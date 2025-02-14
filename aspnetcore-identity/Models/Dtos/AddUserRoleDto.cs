namespace aspnetcore_identity.Models.Dtos;

public class AddUserRoleDto
{
    public Int64 UserId { get; set; }
    public Int64 RoleName { get; set; }
}