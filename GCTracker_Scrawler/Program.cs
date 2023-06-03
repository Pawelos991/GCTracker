﻿using System.Text;
using GCTracker_Scrawler.Scrawler;
using GCTracker_Scrawler.Scrawler.Settings;
using GCTracker_Scrawler.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using GCTracker_Scrawler.Config;
using System;
using GC_Tracker_Datalayer.Model;

const string SCRAWLER_CONFIG_NAME = "scrawler_config.json";


var config = ConfigReader.ReadAppSettings();
var services = new ServiceCollection();
services.InitDataContext(config.ConnectionString);
services.InitDataManipulationServices();
var serviceProvider = services.BuildServiceProvider();

if (TryGetGPUScrawlerSettings(out GPUScrawlerSettings settings))
{
	GPUScrawler scrawler = new (settings);

	List<Product> gpuData = scrawler.GetGPUData();
    LogProductsData(gpuData);

    var product = serviceProvider.GetService<IProductSevices>();
    await product.SaveProductsAsync(gpuData);
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

