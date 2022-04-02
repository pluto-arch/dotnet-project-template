namespace PlutoNetCoreTemplate.Job.Hosting.Controllers
{
    using Infrastructure;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Models;

    using Quartz;

    using System;
    using System.Threading.Tasks;


    /// <summary>
    /// 后台作业
    /// </summary>
    public class TaskController : Controller
    {

        private readonly ISchedulerFactory _jobSchedularFactory;
        private readonly IJobInfoStore _jobInfoStore;
        private readonly IJobLogStore _jobLogStore;

        public TaskController(ISchedulerFactory jobSchedularFactory, IJobInfoStore jobInfoStore, IJobLogStore jobLogStore)
        {
            _jobSchedularFactory = jobSchedularFactory;
            _jobInfoStore = jobInfoStore;
            _jobLogStore = jobLogStore;
        }



        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 获取所有job
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> TasksAsync()
        {
            var jobs = await _jobInfoStore.GetListAsync();
            var scheduler = await _jobSchedularFactory.GetScheduler();
            foreach (var job in jobs)
            {
                var triggers = await scheduler.GetTriggersOfJob(JobKey.Create(job.TaskName, job.GroupName));
                foreach (var trigger in triggers)
                {
                    var dateTimeOffset = trigger.GetPreviousFireTimeUtc();
                    if (!dateTimeOffset.HasValue)
                    {
                        continue;
                    }
                    job.TriggerName = trigger.Key.Name;
                    job.LastRunTime = $"{dateTimeOffset.Value.LocalDateTime:yyyy-MM-dd HH:mm:ss}";
                    job.TriggerStatus = await scheduler.GetTriggerState(trigger.Key);
                }
            }
            return Json(new
            {
                code = 0,
                data = jobs,
                msg = "加载成功",
                count = jobs.Count
            });
        }

        /// <summary>
        /// 暂停任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PauseTaskAsync(PauseTaskRequest request)
        {
            var job = await _jobInfoStore.GetAsync(request.Id);
            if (job == null)
            {
                return Json(new
                {
                    code = -1,
                    msg = "job不存在"
                });
            }
            var scheduler = await _jobSchedularFactory.GetScheduler();
            var jk = JobKey.Create(job.TaskName, job.GroupName);
            await scheduler.PauseJob(jk);
            job.Status = EnumJobStates.Pause;
            await _jobInfoStore.UpdateAsync(job);
            await _jobLogStore.RecordAsync(jk, new JobLogModel
            {
                Time = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                RunSeconds = 0,
                State = EnumJobStates.Pause,
                Message = "暂停指令发送成功"
            });
            return Json(new
            {
                code = 0,
                msg = "操作成功"
            });
        }

        /// <summary>
        /// 重启任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RefireAsync(PauseTaskRequest request)
        {
            var job = await _jobInfoStore.GetAsync(request.Id);
            if (job == null)
            {
                return Json(new
                {
                    code = -1,
                    msg = "job不存在"
                });
            }
            var scheduler = await _jobSchedularFactory.GetScheduler();
            var jk = JobKey.Create(job.TaskName, job.GroupName);
            await scheduler.ResumeJob(jk);
            job.Status = EnumJobStates.Normal;
            await _jobInfoStore.UpdateAsync(job);
            await _jobLogStore.RecordAsync(jk, new JobLogModel
            {
                Time = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                RunSeconds = 0,
                State = EnumJobStates.Normal,
                Message = "重启指令发送成功"
            });
            return Json(new
            {
                code = 0,
                msg = "操作成功"
            });
        }


        /// <summary>
        /// 立即执行
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ExecuteAsync(string id)
        {
            var job = await _jobInfoStore.GetAsync(id);
            if (job == null)
            {
                return Json(new
                {
                    code = -1,
                    msg = "job不存在"
                });
            }
            var jobKey = JobKey.Create(job.TaskName, job.GroupName);
            var scheduler = await _jobSchedularFactory.GetScheduler();
            await scheduler.TriggerJob(jobKey);
            await _jobLogStore.RecordAsync(jobKey, new JobLogModel
            {
                Time = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}",
                RunSeconds = 0,
                State = EnumJobStates.Normal,
                Message = "发送立即执行指令成功"
            });
            return Json(new
            {
                code = 0,
                msg = "操作成功"
            });
        }

        /// <summary>
        /// 作业日志
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> JobLogsAsync(string id)
        {
            var job = await _jobInfoStore.GetAsync(id);
            if (job == null)
            {
                return Json(new
                {
                    code = -1,
                    msg = "job不存在"
                });
            }
            var jk = JobKey.Create(job.TaskName, job.GroupName);
            var logs = await _jobLogStore.GetListAsync(jk, 20);
            return Json(new
            {
                code = 0,
                msg = "操作成功",
                data = logs
            });
        }


    }
}
