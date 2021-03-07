using System.Linq;
using PlutoData.Specifications;
using PlutoData.Specifications.Builder;
using PlutoNetCoreTemplate.Domain.Aggregates.Account;

namespace PlutoNetCoreTemplate.Domain.Services.Account
{
    public class UserSpecification : Specification<UserEntity>
    {
        public UserSpecification(int status)
        {
            Query.Where(x=>x.Id>0);
        }

    }
}
