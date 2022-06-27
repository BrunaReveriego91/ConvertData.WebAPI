using ConvertData.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConvertData.WebAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGenerateFile _generateFile;
        public HomeController(IGenerateFile generateFile)
        {
            _generateFile = generateFile;
        }
        [HttpPost]
        [Route("generate-file")]
        public async Task<IActionResult> SplitFile(IFormFile uploadedFile)
        {
            if (uploadedFile == null || uploadedFile.Length == 0)
                return NotFound();

            var result = await _generateFile.FileSplitWriter(uploadedFile);
            return StatusCode(result.Status, result.Response);
        }
    }
}
