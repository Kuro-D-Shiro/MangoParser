using MangoParser.Services.Interfaces;
using MangoParser.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace MangoParser.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MangoController : ControllerBase
    {
        private readonly IMangaLibParsingService _mangaLibParsingService;
        private readonly IMangaService _mangaService;
        public MangoController(IMangaLibParsingService mangaLibParsingService, IMangaService mangaService)
        {
            _mangaLibParsingService = mangaLibParsingService;
            _mangaService = mangaService;
        }

        [HttpGet("aboba")]
        public async Task<IActionResult> TestAsync()
        {
            await _mangaLibParsingService.ParseAllMangas();

            return Ok();
        }

        [HttpGet("training-data-set")]
        public async Task<IActionResult> GetTrainingDataSet()
        {
            var res = await _mangaService.GetTrainingDataSet();

            return Ok(res);
        }
    }
}
