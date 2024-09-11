using Microsoft.AspNetCore.Mvc;
using WebSignalRApp.Helpers;
using System.Timers; 

namespace WebSignalRApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfferController : ControllerBase
    {
        private static double currentBid = 0;
        private static string lastBidder = string.Empty;
        private static System.Timers.Timer bidTimer; 
        private static readonly object _lock = new();
        private static bool timerActive = false;

        static OfferController()
        {
            bidTimer = new System.Timers.Timer(30000); 
            bidTimer.AutoReset = false;
            bidTimer.Elapsed += OnBidTimerElapsed;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { CurrentBid = currentBid });
        }

        [HttpGet("Increase")]
        public IActionResult Increase(double data, string user)
        {
            lock (_lock)
            {
                if (data <= currentBid && timerActive)
                {
                    return BadRequest("Bid must be higher than the current bid.");
                }

                currentBid = data;
                lastBidder = user;
                timerActive = true;
                bidTimer.Stop();
                bidTimer.Start();
                FileHelper.Write(currentBid);
            }

            return Ok(new { CurrentBid = currentBid });
        }

        private static void OnBidTimerElapsed(object sender, ElapsedEventArgs e)
        {
            lock (_lock)
            {
                timerActive = false;
            }
        }
    }
}
