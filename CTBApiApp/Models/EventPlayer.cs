using System;
using System.Collections.Generic;

namespace CTBApiApp.Models;

public partial class EventPlayer
{
    public int EventPlayerId { get; set; }

    public int EventId { get; set; }

    public int PlayerId { get; set; }

    public int? TopPlece { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;
}
