namespace GCTracker_Scrawler.Scrawler.Settings;

public class GPUScrawlerSettings
{
	public bool RunHeadless { get; set; }
	public List<string> UserAgents { get; set; }
	public List<string> BrowserArguments { get; set; }
	public int MinWaitTime { get; set; }
	public int MaxWaitTime { get; set; }
	public int MaxNumberOfPages { get; set; }
	public int MaxNumberOfElementsPerPage { get; set; }
	public List<SiteSettings> SitesSettings { get; set; }
}