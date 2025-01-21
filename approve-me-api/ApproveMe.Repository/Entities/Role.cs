using System;
using System.Collections.Generic;

namespace ApproveMe.Repository.Entities;

public partial class Role
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }
}
