using System;
using System.Collections.Generic;

namespace ApproveMe.Repository.Entities;

public partial class UserSession
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string Code { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public bool IsValid { get; set; }

    public virtual User User { get; set; } = null!;
}
