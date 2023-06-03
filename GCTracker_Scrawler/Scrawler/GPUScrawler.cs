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
		SlDriver driver = CreateDriver();
		List<Product> products = new ();

		foreach (SiteSettings siteSettings in settings.SitesSettings)
		{
			products.AddRange(GetGPUDataFromSite(driver, siteSettings));
		}

		return products;
	}

	private SlDriver CreateDriver()
	{
		ChromeOptions options = new ();
		options.AddArguments(GetDriverArguments());

		return UndetectedChromeDriver.Instance(DRIVER_PROFILE, options);
	}

	private List<Product> GetGPUDataFromSite(SlDriver driver, SiteSettings siteSettings)
	{
		List<string> productsAddresses = new ();

		driver.Navigate().GoToUrl(siteSettings.SiteURL);
		Wait(driver);

		AcceptCookies(driver, siteSettings);

		int pageNumber = 1;

		do
		{
			productsAddresses.AddRange(GetAddressesFromCurrentPage(driver, siteSettings));
		} while (TryMoveToNextPage(driver, siteSettings, ref pageNumber));

		return GetProductsData(driver, productsAddresses, siteSettings);
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
		//TODO: Handle not found button
		IWebElement nextPageButton = SiteReader.GetPageItem(driver, siteSettings.NextPageButtonSearchData);

		if (currentPageNumber < settings.MaxNumberOfPages)
		{
			nextPageButton.Click();
			currentPageNumber++;
			Wait(driver);

			return true;
		}

		return false;
	}

	private List<Product> GetProductsData(SlDriver driver, List<string> addresses, SiteSettings siteSettings)
	{
		List<Product> products = new ();

		foreach (string address in addresses)
		{
			driver.Navigate().GoToUrl(address);
			Wait(driver);

			products.Add(GetProductData(driver, siteSettings));
		}

		return products;
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
		int agentIndex = new Random().Next(settings.UserAgents.Count + 1);
		return settings.UserAgents[agentIndex];
	}

	private void Wait(SlDriver driver)
	{
		driver.RandomWaitMiliseconds(settings.MinWaitTime, settings.MaxWaitTime);
	}
}