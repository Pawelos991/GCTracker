using System.Text;
using GCTracker_Scrawler.Scrawler;
using GCTracker_Scrawler.Scrawler.Data;
using GCTracker_Scrawler.Scrawler.Settings;
using Newtonsoft.Json;

const string SCRAWLER_CONFIG_NAME = "scrawler_config.json";

if (TryGetGPUScrawlerSettings(out GPUScrawlerSettings settings))
{
	GPUScrawler scrawler = new (settings);
	List<ProductData> gpuData = scrawler.GetGPUData();

	LogProductsData(gpuData);
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

static void LogProductsData(List<ProductData> products)
{
	foreach (ProductData product in products)
	{
		Console.WriteLine($"Name: {product.Name}");
		Console.WriteLine($"Price: {product.Price}");
		Console.WriteLine($"ProducentCode: {product.ProducentCode}");
		Console.WriteLine($"ImageAddress: {product.ImageAddress}");
		Console.WriteLine($"StoreName: {product.StoreName}");
	}
}