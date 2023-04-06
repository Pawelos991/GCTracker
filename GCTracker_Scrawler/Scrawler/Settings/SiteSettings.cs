namespace GCTracker_Scrawler.Scrawler.Settings;

public class SiteSettings
{
	public string SiteURL { get; set; }
	public string NextPageButtonXPath { get; set; }
	public ItemSearchMethod ItemSearchMethod { get; set; }
	public string ItemSearchPhrase { get; set; }
	public string NameXPath { get; set; }
	public List<string> PriceXPaths { get; set; }
	public string ProducentCodeXPath { get; set; }
	public string ImageAddressXPath { get; set; }
	public string CookieButtonXPath { get; set; }
}