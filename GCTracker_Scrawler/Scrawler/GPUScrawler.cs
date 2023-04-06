using System.ComponentModel;
using GCTracker_Scrawler.Scrawler.Data;
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

		public List<ProductData> GetGPUData()
		{
			SlDriver driver = CreateDriver();
			List<ProductData> products = new ();

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

		private List<ProductData> GetGPUDataFromSite(SlDriver driver, SiteSettings siteSettings)
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
				IWebElement cookieButton = driver.FindElement(By.XPath(siteSettings.CookieButtonXPath));

				cookieButton.Click();
				Wait(driver);
			}
			catch (Exception e)
			{
				Console.WriteLine($"Exception occured, it is possible that the cookies have already been accepted or the accept button was not found. {e.Message}");
			}
		}

		private List<string> GetAddressesFromCurrentPage(ISearchContext driver, SiteSettings siteSettings)
		{
			List<string> addresses = new ();
			ItemSearchMethod searchMethod = siteSettings.ItemSearchMethod;

			IReadOnlyCollection<IWebElement> productList = searchMethod switch
			{
				ItemSearchMethod.BY_TEXT => driver.FindElements(By.LinkText(siteSettings.ItemSearchPhrase)),
				ItemSearchMethod.BY_CLASS => driver.FindElements(By.ClassName(siteSettings.ItemSearchPhrase)),
				_ => throw new InvalidEnumArgumentException(nameof(searchMethod), (int) searchMethod, typeof(ItemSearchMethod))
			};

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
			IWebElement nextPageButton = driver.FindElement(By.XPath(siteSettings.NextPageButtonXPath));

			if (nextPageButton != null && currentPageNumber < settings.MaxNumberOfPages)
			{
				nextPageButton.Click();
				currentPageNumber++;
				Wait(driver);

				return true;
			}

			return false;
		}

		private List<ProductData> GetProductsData(SlDriver driver, List<string> addresses, SiteSettings siteSettings)
		{
			List<ProductData> products = new ();

			foreach (string address in addresses)
			{
				driver.Navigate().GoToUrl(address);
				Wait(driver);

				products.Add(GetProductData(driver, siteSettings));
			}

			return products;
		}

		private ProductData GetProductData(ISearchContext driver, SiteSettings siteSettings)
		{
			ProductData product = new ()
			{
				Name = driver.FindElement(By.XPath(siteSettings.NameXPath)).Text,
				Price = GetElementByXPathCollection(driver, siteSettings.PriceXPaths).Text,
				ProducentCode = driver.FindElement(By.XPath(siteSettings.ProducentCodeXPath)).Text,
				ImageAddress = driver.FindElement(By.XPath(siteSettings.ImageAddressXPath)).GetAttribute("src")
			};

			return product;
		}

		private IWebElement GetElementByXPathCollection(ISearchContext driver, List<string> xPathCollection)
		{
			foreach (string xPath in xPathCollection)
			{
				try
				{
					return driver.FindElement(By.XPath(xPath));
				}
				catch (NoSuchElementException e)
				{
					Console.WriteLine($"Element could not be found with XPath: {xPath}");
				}
			}

			throw new NoSuchElementException();
		}

		private List<string> GetDriverArguments()
		{
			List<string> arguments = settings.BrowserArguments;

			if (settings.RunHeadless)
			{
				arguments.AddRange(new List<string>()
				{
					"headless",
					"disable-gpu",
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