// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PlutoNetCoreTemplate.Job.Hosting.Controllers
{
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using Quartz;
    using System.Linq;
    using System.Threading.Tasks;

    public class HomeController : Controller
    {
        private readonly ISchedulerFactory _jobSchedularFactory;
        private readonly IJobInfoStore _jobInfoStore;

        public HomeController(ISchedulerFactory jobSchedularFactory, IJobInfoStore jobInfoStore)
        {
            _jobSchedularFactory = jobSchedularFactory;
            _jobInfoStore = jobInfoStore;
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
