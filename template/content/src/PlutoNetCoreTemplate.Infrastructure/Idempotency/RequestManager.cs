using System;
using System.Threading.Tasks;

namespace PlutoNetCoreTemplate.Infrastructure.Idempotency
{
    public class RequestManager : IRequestManager
    {
        public Task<bool> ExistAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task CreateRequestForCommandAsync<T>(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}