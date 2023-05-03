using System;
using System.Collections.Generic;

namespace CTBApiApp.Models;

public partial class Administrator
{
    public int AdministratorId { get; set; }

    public int OrganizerId { get; set; }

    public virtual Organizer Organizer { get; set; } = null!;
}
