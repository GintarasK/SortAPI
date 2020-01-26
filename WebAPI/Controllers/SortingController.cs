using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Sort;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;

namespace WebAPI.Controllers
{

    [ApiController]
    [Route("api/")]
    public class SortingController : ControllerBase
    {
        private readonly ISortService sortService;
        private readonly ILogger<SortingController> _logger;

        private static string fileName = "result.txt";
        private readonly string fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), fileName);

        public SortingController(
            ILogger<SortingController> logger,
            ISortService sortService)
        {
            _logger = logger;
            this.sortService = sortService;
        }

        [Route("Sort")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Sort([FromQuery(Name = "array")] string arrayStirng)
        {
            try
            {
                var array = arrayStirng
                    ?.Split(' ')
                    .Select(q =>
                    {
                        if (int.TryParse(q, out var result))
                        {
                            return result;
                        }
                        else
                        {
                            throw new ValidationException($"parameter {nameof(arrayStirng)} does not contain only numbers");
                        }
                    })
                    .ToList();

                var sortedList = sortService.BubbleMethod(array);

                using (StreamWriter outputFile = new StreamWriter(fileDirectory))
                {
                    outputFile.WriteLine(string.Join(' ', sortedList));
                }

                return new JsonResult(new { Array = sortedList });
            }
            catch (ValidationException e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("GetFile")]
        [HttpGet]
        public ActionResult GetLatestFile()
        {
            StreamReader streamReader = new StreamReader(fileDirectory);

            if (streamReader == null)
            {
                return BadRequest("File has not yet been generated");
            }

            return File(streamReader.BaseStream, "text/plain", fileName);
        }
    }
}
