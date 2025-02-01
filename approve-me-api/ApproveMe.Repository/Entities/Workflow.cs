using System;
using System.Collections.Generic;

namespace ApproveMe.Repository.Entities;

public partial class Workflow
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public int Sequence { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<LetterType> LetterTypes { get; set; } = new List<LetterType>();

    public virtual User User { get; set; } = null!;
}
