using System.Linq;
using PlutoData.Specifications;
using PlutoData.Specifications.Builder;

namespace PlutoNetCoreTemplate.Domain.Services.Account
{
    using Aggregates.System;

    public sealed class UserSpecification : Specification<UserEntity>
    {
        public UserSpecification()
        {
            Query.Where(x=>x.Id>0);
        }

    }
}
