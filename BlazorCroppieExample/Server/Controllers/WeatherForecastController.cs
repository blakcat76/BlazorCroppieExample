using BlazorCroppieExample.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BlazorCroppieExample.Server.Controllers
{    
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private IWebHostEnvironment _environment;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;

        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<ActionResult> PostFile(FileUpload model)
        {
            try
            {
                string name = Guid.NewGuid().ToString();
                string file = null;
                if (!string.IsNullOrEmpty(model.Avatar))
                {
                    ProcessImage(model.Avatar, name);
                    file = "Uploads/" + name + ".png";
                }
                return Ok();
            }
            catch
            {
                return Problem();
            }            
        }

        private string ProcessImage(string croppedImage, string name)
        {
            string filePath = String.Empty;

            try
            {
                string base64 = croppedImage;
                byte[] bytes = Convert.FromBase64String(base64.Split(',')[1]);

                filePath = Path.Combine("Uploads/" + name + ".png");

                using (FileStream stream = new FileStream(Path.Combine(_environment.WebRootPath, filePath), FileMode.Create))
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                }
            }
            catch (Exception ex)
            {
                string st = ex.Message;
            }
            return filePath;
        }

    }
}