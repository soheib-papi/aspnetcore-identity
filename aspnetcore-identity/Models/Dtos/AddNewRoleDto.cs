namespace aspnetcore_identity.Models.Dtos;

public class AddNewRoleDto
{
    public required string Name { get; set; }
    public string Description { get; set; }
}