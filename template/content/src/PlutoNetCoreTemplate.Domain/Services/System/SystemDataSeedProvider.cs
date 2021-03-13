using System;
using System.Threading.Tasks;
using PlutoData.Interface;
using PlutoNetCoreTemplate.Domain.SeedWork;

namespace PlutoNetCoreTemplate.Domain.Services.Account
{
    using Aggregates.System;

    public class SystemDataSeedProvider : IDataSeedProvider
    {
        private readonly IEfRepository<UserEntity> _userRepository;

        public SystemDataSeedProvider(IEfRepository<UserEntity> userRepository)
        {
            _userRepository = userRepository;
        }


        public async Task SeedAsync(IServiceProvider serviceProvider)
        {
            if (await _userRepository.LongCountAsync() <= 0)
            {
                for (int i = 0; i < 20; i++)
                {
                    var user = new UserEntity
                    {
                        UserName = $"UserEntity_{i}",
                        Email = $"UserEntity_{i}" + "@qq.com"
                    };
                    await _userRepository.InsertAsync(user);
                    //user.AddDomainEvent(new ProductCreatedDomainEvent(user));
                }
            }
        }

    }
}
