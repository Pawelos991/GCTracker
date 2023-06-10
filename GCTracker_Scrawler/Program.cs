using System.Net.Mime;
using System.Text;
using GCTracker_Scrawler.Scrawler;
using GCTracker_Scrawler.Scrawler.Settings;
using GCTracker_Scrawler.Services;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using GCTracker_Scrawler.Config;
using GC_Tracker_Datalayer.Model;
using static System.Net.WebRequestMethods;

const string SCRAWLER_CONFIG_NAME = "scrawler_config.json";
const int COLLECTING_DATA_DELAY = 7200000;

var config = ConfigReader.ReadAppSettings();
var services = new ServiceCollection();
services.InitDataContext(config.ConnectionString);
services.InitDataManipulationServices();
var serviceProvider = services.BuildServiceProvider();

if (TryGetGPUScrawlerSettings(out GPUScrawlerSettings settings))
{
	GPUScrawler scrawler = new (settings);
    var product = serviceProvider.GetService<IProductSevices>();
    var imagesService = serviceProvider.GetService<IImageServices>();

    Timer timer = new Timer(async _ =>
	{
		List<Product> gpuData = scrawler.GetGPUData();
		LogProductsData(gpuData);
		var savedProd = await product.SaveProductsAsync(gpuData);
        foreach (var gpu in savedProd)
        {
			if (gpu.Id > 0 && !String.IsNullOrEmpty(gpu.ImageAddress) && !(await imagesService.CheckIsImageExistInDatabase(gpu.ProducentCode)))
            {
                var image = await imagesService.GetImageByUrl(gpu.ImageAddress);
                Images imageToSave = new Images()
                {
					Img = image,
                    Imgsmall = image,
                    ProducentCode = gpu.ProducentCode,
					ProductId = gpu.Id,
                };

                await imagesService.SaveImages(imageToSave);
            }
        }
		
		Console.WriteLine($"Added {gpuData.Count} products to database.");
		Console.WriteLine($"Waiting for the next download data  process (remaining time: {COLLECTING_DATA_DELAY} milliseconds.)");
	}, null, 0, COLLECTING_DATA_DELAY);
	
	Thread.Sleep(Timeout.Infinite);
}

static bool TryGetGPUScrawlerSettings(out GPUScrawlerSettings settings)
{
	settings = null;

	using (StreamReader reader = new StreamReader(SCRAWLER_CONFIG_NAME, Encoding.UTF8))
	{
		settings = JsonConvert.DeserializeObject<GPUScrawlerSettings>(reader.ReadToEnd());
	}

	return settings != null;
}

static void LogProductsData(List<Product> products)
{
	foreach (Product product in products)
	{
		Console.WriteLine($"Name: {product.Name}");
		Console.WriteLine($"Price: {product.Price}");
		Console.WriteLine($"ProducentCode: {product.ProducentCode}");
		Console.WriteLine($"ImageAddress: {product.ImageAddress}");
		Console.WriteLine($"StoreName: {product.StoreName}");
	}
}
