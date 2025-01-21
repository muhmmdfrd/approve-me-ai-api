using System;
using System.Collections.Generic;

namespace ApproveMe.Repository.Entities;

public partial class User
{
    public long Id { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string Password { get; set; } = null!;

    public long? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
}
