using System;
using System.Collections.Generic;

namespace GC_Tracker_Datalayer.Model;

public partial class Scraper
{
    public int Id { get; set; }

    public decimal Price { get; set; }

    public string Name { get; set; } = null!;

    public string? PhotoLink { get; set; }

    public string PageName { get; set; } = null!;

    public DateTime? Created { get; set; }

    public string? CreatedBy { get; set; }
}
