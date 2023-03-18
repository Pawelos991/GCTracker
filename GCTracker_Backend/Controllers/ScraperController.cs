using GC_Tracker_Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GCTracker_Backend.Controllers
{
    public class ScraperController : Controller
    {
        private readonly IScraperServices _scraperServices;

        public ScraperController(IScraperServices scraper)
        {
            _scraperServices = scraper;
        }

        [HttpGet("api/scraper")]
        public async Task<IActionResult> GetScrapper()
        {
            var resultToRet = await _scraperServices.GetScraperDataAsync();
            return Ok(resultToRet);
        }
    }
}
