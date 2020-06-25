using System.Threading.Tasks;

namespace Demo.Application.GrpcServices.GrpcCallers.Interfaces
{
    public interface IOrderGrpcService
    {
        Task<object> Get();
    }
}