namespace PlutoNetCoreTemplate.Application.AppServices.ProductAppServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.ProductModels;

    public interface IProductAppService
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        Task<List<ProductModels>> GetListAsync();

        /// <summary>
        /// 获取单个
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<ProductModels> GetAsync(string key);
    }
}