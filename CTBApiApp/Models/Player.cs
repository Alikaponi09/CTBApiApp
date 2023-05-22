using System;
using System.Collections.Generic;

namespace CTBApiApp.Models;

public partial class Player
{
    public int Fideid { get; set; }

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string? LastName { get; set; }

    public DateTime Birthday { get; set; }

    public double Elorating { get; set; }

    public string Contry { get; set; } = null!;

    public string Passord { get; set; } = null!;

    public byte[]? Image { get; set; }

    public virtual ICollection<ConsignmentPlayer> ConsignmentPlayers { get; set; } = new List<ConsignmentPlayer>();

    public virtual ICollection<EventPlayer> EventPlayers { get; set; } = new List<EventPlayer>();
}
