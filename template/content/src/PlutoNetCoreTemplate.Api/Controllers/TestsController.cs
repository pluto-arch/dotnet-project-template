namespace PlutoNetCoreTemplate.Api.Controllers
{
    using Domain.SeedWork;
    using Filters;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using System.ComponentModel.DataAnnotations;
    using System.Reflection;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : BaseController<TestsController>
    {

        private readonly IStringLocalizer _stringLocalizer; // 字符串本地化

        public TestsController(IStringLocalizerFactory stringLocalizer, ILazyLoadServiceProvider lazyLoad) : base(lazyLoad)
        {
            _stringLocalizer = stringLocalizer.Create("Welcome", Assembly.GetExecutingAssembly().ToString());
        }


        /// <summary>
        /// Firewall
        /// </summary>
        /// <returns></returns>
        [HttpGet("getList")]
        [Firewall(Limit = 10)]
        public async Task<ServiceResponse<string>> GetListAsync()
        {
            await Task.Delay(20);
            return ServiceResponse<string>.Success(_stringLocalizer["HelloWorld"]);
        }


        /// <summary>
        /// Firewall
        /// </summary>
        /// <returns></returns>
        [HttpGet("get")]
        [Firewall(Limit = 10)]
        [ProducesDefaultResponseType(typeof(ServiceResponse<string>))]
        public async Task<IActionResult> GetAsync([Required] int id)
        {
            await Task.Delay(1);
            if (id % 2 == 0)
            {
                return Ok(ServiceResponse<string>.Success("123123123"));
            }
            return Ok("123123123");
        }
    }
}
