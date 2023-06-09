using GC_Tracker_Datalayer.Model;
using GCTracker_Scrawler.Scrawler.Settings;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Selenium.Extensions;
using Selenium.WebDriver.UndetectedChromeDriver;

namespace GCTracker_Scrawler.Scrawler;

public class GPUScrawler
{
	private readonly GPUScrawlerSettings settings;

	private const string DRIVER_PROFILE = "profile_1";

	public GPUScrawler(GPUScrawlerSettings settings)
	{
		this.settings = settings;
	}

	public List<Product> GetGPUData()
	{
		Console.WriteLine("Start collecting GPU data.");
		SlDriver driver = CreateDriver();
		List<Product> products = new ();

		try
		{
			foreach (SiteSettings siteSettings in settings.SitesSettings)
			{
				GetGPUDataFromSite(driver, siteSettings, products);
			}
			
			Console.WriteLine("GPU Data is collected successfully.");
		}
		catch (WebDriverException e)
		{
			Console.WriteLine(e);
			Console.WriteLine($"{nameof(WebDriverException)} occurred, data collection was aborted.");
		}
		
		return products;
	}

	private SlDriver CreateDriver()
	{
		ChromeOptions options = new ();
		options.AddArguments(GetDriverArguments());

		return UndetectedChromeDriver.Instance(DRIVER_PROFILE, options);
	}

	private void GetGPUDataFromSite(SlDriver driver, SiteSettings siteSettings, List<Product> productsOutput)
	{
		Console.WriteLine($"Collecting GPU data from {siteSettings.SiteName} site.");
		
		List<string> productsAddresses = new ();

		driver.Navigate().GoToUrl(siteSettings.SiteURL);
		Wait(driver);

		AcceptCookies(driver, siteSettings);

		int pageNumber = 1;

		do
		{
			productsAddresses.AddRange(GetAddressesFromCurrentPage(driver, siteSettings));
		} while (TryMoveToNextPage(driver, siteSettings, ref pageNumber));

		GetProductsData(driver, productsAddresses, siteSettings, productsOutput);
	}

	private void AcceptCookies(SlDriver driver, SiteSettings siteSettings)
	{
		//TODO: Replace additional Wait() call with waiting for page to load.
		Wait(driver);

		try
		{
			IWebElement cookieButton = SiteReader.GetPageItem(driver, siteSettings.CookieButtonSearchData);

			cookieButton.Click();
			Wait(driver);
		}
		catch (Exception e)
		{
			Console.WriteLine($"Exception occured, it is possible that the cookies have already been accepted or the accept button was not found. {e.Message}");
		}
	}

	//TODO: Add handling not found elements
	private List<string> GetAddressesFromCurrentPage(SlDriver driver, SiteSettings siteSettings)
	{
		List<string> addresses = new ();
		IReadOnlyCollection<IWebElement> productList = SiteReader.GetPageItems(driver, siteSettings.ItemSearchData);

		foreach (IWebElement product in productList)
		{
			if (addresses.Count < settings.MaxNumberOfElementsPerPage)
			{
				addresses.Add(product.GetAttribute("href"));
			}
			else
			{
				break;
			}
		}

		return addresses;
	}

	private bool TryMoveToNextPage(SlDriver driver, SiteSettings siteSettings, ref int currentPageNumber)
	{
		try
		{
			IWebElement nextPageButton = SiteReader.GetPageItem(driver, siteSettings.NextPageButtonSearchData);

			if (currentPageNumber < settings.MaxNumberOfPages)
			{
				nextPageButton.Click();
				currentPageNumber++;
				Wait(driver);

				Console.WriteLine($"Moving to page {currentPageNumber}.");
				return true;
			}

			return false;
		}
		catch (NoSuchElementException exception)
		{
			Console.WriteLine("Move to next page button is not found.");
			return false;
		}
	}

	private void GetProductsData(SlDriver driver, List<string> addresses, SiteSettings siteSettings, List<Product> productsOutput)
	{
		Console.WriteLine("Collecting products data.");

		foreach (string address in addresses)
		{
			Console.WriteLine($"Opening site with product data: {address}");
			driver.Navigate().GoToUrl(address);
			Console.WriteLine($"Reading product data...");
			Wait(driver);

			productsOutput.Add(GetProductData(driver, siteSettings));
		}
	}

	private Product GetProductData(SlDriver driver, SiteSettings siteSettings)
	{
		Product product = new ()
		{
			Name = DataProcessor.ProcessCardNameValue(SiteReader.GetPageItem(driver, siteSettings.NameSearchData).GetAttribute("innerText")),
			Price = DataProcessor.ProcessPriceValue(SiteReader.GetPageItem(driver, siteSettings.PriceSearchData).GetAttribute("innerText")),
			ProducentCode = DataProcessor.ProcessProducentCodeValue(SiteReader.GetPageItem(driver, siteSettings.ProducerSearchData).GetAttribute("innerText")),
			ImageAddress = SiteReader.GetPageItem(driver, siteSettings.ImageSearchData).GetAttribute("src"),
			StoreName = siteSettings.SiteName
		};

		return product;
	}

	private List<string> GetDriverArguments()
	{
		List<string> arguments = settings.BrowserArguments;

		if (settings.RunHeadless)
		{
			arguments.AddRange(new List<string>()
			{
				"--no-sandbox",
				"--headless",
				"--disable-gpu",
				"--disable-dev-shm-usage",
				$"--user-agent={GetRandomUserAgent()}"
			});
		}

		return arguments;
	}

	private string GetRandomUserAgent()
	{
		int agentIndex = new Random().Next(settings.UserAgents.Count);
		return settings.UserAgents[agentIndex];
	}

	private void Wait(SlDriver driver)
	{
		driver.RandomWaitMiliseconds(settings.MinWaitTime, settings.MaxWaitTime);
	}
}