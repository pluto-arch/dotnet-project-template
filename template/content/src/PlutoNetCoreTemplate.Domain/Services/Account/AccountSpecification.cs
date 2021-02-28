using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlutoData.Specifications;
using PlutoData.Specifications.Builder;

using PlutoNetCoreTemplate.Domain.DomainModels.Account;

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
