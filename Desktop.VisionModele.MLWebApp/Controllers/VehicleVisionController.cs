using Desktop_VisionModele_MLWebApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ML;

namespace Desktop.VisionModele.MLWebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehicleVisionController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<VehicleVisionController> _logger;

        public VehicleVisionController(ILogger<VehicleVisionController> logger
            , PredictionEnginePool<VehicleMLModel.ModelInput, VehicleMLModel.ModelOutput> predictionEnginePool)
        {
            _logger = logger;
            this.predictionEnginePool = predictionEnginePool;
        }

        //[HttpGet(Name = "GetWeatherForecast")]
        //public IEnumerable<WeatherForecast> Get()
        //{
        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        //    {
        //        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
        //        TemperatureC = Random.Shared.Next(-20, 55),
        //        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        //    })
        //    .ToArray();
        //}

        public PredictionEnginePool<VehicleMLModel.ModelInput, VehicleMLModel.ModelOutput> predictionEnginePool;


        [HttpPost(Name = "predict")]
        public async Task<VehicleMLModel.ModelOutput> predict(string imagePath)
        {
            //imagePath = @"C:\AudiCar\Body\IMG_20250328_140232.jpg";
            System.Diagnostics.Debug.Print($"{imagePath}");

            var input = new VehicleMLModel.ModelInput()
            {
                //var imageBytes = File.ReadAllBytes(@"C:\AudiCar\Body\IMG_20250328_140232.jpg");

                ImageSource = System.IO.File.ReadAllBytes(imagePath),
            };
            System.Diagnostics.Debug.Print("·µ»Ø½á¹û");
            return await Task.FromResult(predictionEnginePool.Predict(input));


        }




    }
}
