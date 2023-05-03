using System;
using System.Collections.Generic;

namespace CTBApiApp.Models;

public partial class Consignment
{
    public int ConsignmentId { get; set; }

    public DateTime DateStart { get; set; }

    public int TourId { get; set; }

    public int StatusId { get; set; }

    public string? GameMove { get; set; }

    public string? TableName { get; set; }

    public virtual ICollection<ConsignmentPlayer> ConsignmentPlayers { get; set; } = new List<ConsignmentPlayer>();

    public virtual Status Status { get; set; } = null!;

    public virtual Tour Tour { get; set; } = null!;
}
