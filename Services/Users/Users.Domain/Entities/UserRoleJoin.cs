namespace Users.Domain.Entities;

public class UserRoleJoin
{
    public Guid UserId { get; set; }
    public int RoleId { get; set; }
}