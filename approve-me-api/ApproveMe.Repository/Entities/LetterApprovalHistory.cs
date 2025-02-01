using System;
using System.Collections.Generic;

namespace ApproveMe.Repository.Entities;

public partial class LetterApprovalHistory
{
    public long Id { get; set; }

    public long LetterApprovalId { get; set; }

    public string? Token { get; set; }

    public long LetterId { get; set; }

    public int StatusId { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Letter Letter { get; set; } = null!;

    public virtual LetterApproval LetterApproval { get; set; } = null!;
}
