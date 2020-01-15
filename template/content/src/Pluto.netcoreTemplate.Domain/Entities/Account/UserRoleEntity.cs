using Pluto.netcoreTemplate.Domain.SeedWork;


namespace Pluto.netcoreTemplate.Domain.Entities.Account
{
    public class UserRoleEntity : IAggregateRoot
    {
        public int UserId { get; set; }
        public UserEntity User { get; set; }


        public int RoleId { get; set; }
        public RoleEntity Role { get; set; }
    }
}