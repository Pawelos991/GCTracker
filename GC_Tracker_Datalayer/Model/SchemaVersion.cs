using System;
using System.Collections.Generic;

namespace GC_Tracker_Datalayer.Model;

public partial class SchemaVersion
{
    public int Id { get; set; }

    public string ScriptName { get; set; } = null!;

    public DateTime Applied { get; set; }
}
