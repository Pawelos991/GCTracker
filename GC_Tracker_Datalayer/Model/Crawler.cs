using System;
using System.Collections.Generic;

namespace GC_Tracker_Datalayer.Model;

public partial class Crawler
{
    public int Id { get; set; }

    public string CrawlerLink { get; set; } = null!;

    public string PageName { get; set; } = null!;

    public DateTime? Created { get; set; }

    public string? CreatedBy { get; set; }
}
