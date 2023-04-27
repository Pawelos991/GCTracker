namespace GCTracker_Scrawler.Scrawler.Settings;

public class SiteSettings
{
	public string SiteName { get; set; }
	public string SiteURL { get; set; }
	public SearchData NextPageButtonSearchData { get; set; }
	public SearchData ItemSearchData { get; set; }
	public SearchData NameSearchData { get; set; }
	public SearchData PriceSearchData { get; set; }
	public SearchData ProducerSearchData { get; set; }
	public SearchData ImageSearchData { get; set; }
	public SearchData CookieButtonSearchData { get; set; }
}