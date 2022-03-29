// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlutoNetCoreTemplate.Job.Hosting.Controllers
{
    using Domain.Aggregates.TenantAggregate;
    using Domain.Repositories;

    using Infrastructure;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Models;

    using Quartz;

    using System.Linq;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        private readonly ISchedulerFactory _jobSchedularFactory;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IJobInfoStore _jobInfoStore;

        private readonly IRepository<Tenant> _tenants;

        public HomeController(ISchedulerFactory jobSchedularFactory, ILogger<HomeController> logger, IConfiguration configuration, IJobInfoStore jobInfoStore, IRepository<Tenant> tenants)
        {
            _jobSchedularFactory = jobSchedularFactory;
            _logger = logger;
            _configuration = configuration;
            _jobInfoStore = jobInfoStore;
            _tenants = tenants;
        }


        public async Task<IActionResult> Index()
        {
            var scheduler = await _jobSchedularFactory.GetScheduler();
            var jobs = await scheduler.GetCurrentlyExecutingJobs();
            ViewData["JobCount"] = await _jobInfoStore.CountAsync();
            ViewData["RunningJobCount"] = jobs.Count;
            ViewData["PauseJobCount"] = (await _jobInfoStore.GetListAsync()).Count(x => x.Status == EnumJobStates.Pause);
            return View();
        }
    }
}
