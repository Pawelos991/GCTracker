using System.ComponentModel;
using OpenQA.Selenium;
using Selenium.Extensions;

namespace GCTracker_Scrawler.Scrawler;

public static class SiteReader
{
	public static IWebElement GetPageItem(SlDriver driver, SearchData searchData)
	{
		ItemSearchMethod searchMethod = searchData.ItemSearchMethod;
		
		IWebElement element = searchMethod switch
		{
			ItemSearchMethod.BY_TEXT => driver.FindElement(By.LinkText(searchData.Text)),
			ItemSearchMethod.BY_CLASS => driver.FindElement(By.ClassName(searchData.Text)),
			ItemSearchMethod.BY_ID => driver.FindElement(By.Id(searchData.Text)),
			ItemSearchMethod.BY_XPATH => driver.FindElement(By.XPath(searchData.Text)),
			ItemSearchMethod.BY_CSS_SELECTOR => driver.FindElement(By.CssSelector(searchData.Text)),
			_ => throw new InvalidEnumArgumentException(nameof(searchMethod), (int) searchMethod, typeof(ItemSearchMethod))
		};

		return searchData.GetParent ? element.FindElement(By.XPath("./..")) : element;
	}
	
	public static IReadOnlyCollection<IWebElement> GetPageItems(SlDriver driver, SearchData searchData)
	{
		ItemSearchMethod searchMethod = searchData.ItemSearchMethod;
		
		return searchMethod switch
		{
			ItemSearchMethod.BY_TEXT => driver.FindElements(By.LinkText(searchData.Text)),
			ItemSearchMethod.BY_CLASS => driver.FindElements(By.ClassName(searchData.Text)),
			ItemSearchMethod.BY_ID => driver.FindElements(By.Id(searchData.Text)),
			ItemSearchMethod.BY_XPATH => driver.FindElements(By.XPath(searchData.Text)),
			_ => throw new InvalidEnumArgumentException(nameof(searchMethod), (int) searchMethod, typeof(ItemSearchMethod))
		};
	}
}