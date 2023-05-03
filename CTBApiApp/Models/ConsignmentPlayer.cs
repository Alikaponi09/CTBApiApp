using System;
using System.Collections.Generic;

namespace CTBApiApp.Models;

public partial class ConsignmentPlayer
{
    public int ConsignmentPlayerId { get; set; }

    public int ConsignmentId { get; set; }

    public int PlayerId { get; set; }

    public bool IsWhile { get; set; }

    public double? Result { get; set; }

    public virtual Consignment Consignment { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;
}
