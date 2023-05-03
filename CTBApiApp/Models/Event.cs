using System;
using System.Collections.Generic;

namespace CTBApiApp.Models;

public partial class Event
{
    public int EventId { get; set; }

    public string Name { get; set; } = null!;

    public int PrizeFund { get; set; }

    public string LocationEvent { get; set; } = null!;

    public DateTime DataStart { get; set; }

    public DateTime DataFinish { get; set; }

    public int StatusId { get; set; }

    public int OrganizerId { get; set; }

    public bool IsPublic { get; set; }

    public bool TypeEvent { get; set; }

    public byte[]? Image { get; set; }

    public virtual ICollection<EventPlayer> EventPlayers { get; set; } = new List<EventPlayer>();

    public virtual Organizer Organizer { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual ICollection<Tour> Tours { get; set; } = new List<Tour>();
}
