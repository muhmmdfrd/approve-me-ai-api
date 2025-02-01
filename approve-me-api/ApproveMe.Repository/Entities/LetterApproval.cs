using System;
using System.Collections.Generic;

namespace ApproveMe.Repository.Entities;

public partial class LetterApproval
{
    public long Id { get; set; }

    public string? Token { get; set; }

    public long LetterId { get; set; }

    public int StatusId { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public long ApproverUserId { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual User ApproverUser { get; set; } = null!;

    public virtual Letter Letter { get; set; } = null!;

    public virtual ICollection<LetterApprovalHistory> LetterApprovalHistories { get; set; } = new List<LetterApprovalHistory>();
}
