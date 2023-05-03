using System;
using System.Collections.Generic;

namespace CTBApiApp.Models;

public partial class Status
{
    public int StatusId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Consignment> Consignments { get; set; } = new List<Consignment>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
