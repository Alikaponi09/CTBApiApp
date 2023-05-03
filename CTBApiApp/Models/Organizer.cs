using System;
using System.Collections.Generic;

namespace CTBApiApp.Models;

public partial class Organizer
{
    public int OrganizerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string? LastName { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public byte[]? Image { get; set; }

    public virtual ICollection<Administrator> Administrators { get; set; } = new List<Administrator>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
