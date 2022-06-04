using Hangfire;
using Microsoft.AspNetCore.Mvc;
using System;

namespace NotificationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : Controller
    {
        [HttpPost]
        [Route("fire-and-forget")]
        public IActionResult FireAndForget(string client)
        {
            string jobId = BackgroundJob.Enqueue(() =>
                Console.WriteLine($"{client}, thank you for contacting us."));

            return Ok($"{jobId}");
        }

        [HttpPost]
        [Route("delayed")]
        public IActionResult DelayedJob(string client)
        {
            string jobId = BackgroundJob.Schedule(() =>
                Console.WriteLine($"Session for client {client} has been closed."),
                TimeSpan.FromSeconds(5));

            return Ok($"{jobId}");
        }

        [HttpPost]
        [Route("recurring")]
        public IActionResult Recurring()
        {
            RecurringJob.AddOrUpdate(() => 
                Console.WriteLine("It's high time to work!"), Cron.Daily);

            return Ok();
        }

        [HttpPost]
        [Route("continuations")]
        public IActionResult Continuations(string client)
        {
            string jobId = BackgroundJob.Enqueue(() =>
                Console.WriteLine($"Check balance for {client}"));

            BackgroundJob.ContinueJobWith(jobId, () =>
                Console.WriteLine($"{client}, your balance has been changed"));

            return Ok();
        }
    }
}
