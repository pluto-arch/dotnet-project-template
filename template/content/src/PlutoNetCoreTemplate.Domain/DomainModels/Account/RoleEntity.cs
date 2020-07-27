using System.Collections.Generic;

namespace PlutoNetCoreTemplate.Domain.DomainModels.Account
{
    public class RoleEntity : BaseEntity<int>
    {

        public RoleEntity()
        { }

        public RoleEntity(string name)
        {
            this.RoleName = RoleName;
        }

        public string RoleName { get; set; }

        public IReadOnlyCollection<UserRoleEntity> Users { get; set; }
    }
}