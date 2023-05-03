using System;
using System.Collections.Generic;

namespace CTBApiApp.Models;

public partial class Tour
{
    public int TourId { get; set; }

    public string NameTour { get; set; } = null!;

    public int EventId { get; set; }

    public virtual ICollection<Consignment> Consignments { get; set; } = new List<Consignment>();

    public virtual Event Event { get; set; } = null!;
}