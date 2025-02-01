using System;
using System.Collections.Generic;

namespace ApproveMe.Repository.Entities;

public partial class Letter
{
    public long Id { get; set; }

    public long CreatorId { get; set; }

    public long LetterTypeId { get; set; }

    public DateTime LetterDate { get; set; }

    public int StatusId { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public string Title { get; set; } = null!;

    public virtual User Creator { get; set; } = null!;

    public virtual ICollection<LetterApprovalHistory> LetterApprovalHistories { get; set; } = new List<LetterApprovalHistory>();

    public virtual ICollection<LetterApproval> LetterApprovals { get; set; } = new List<LetterApproval>();

    public virtual ICollection<LetterAttachment> LetterAttachments { get; set; } = new List<LetterAttachment>();

    public virtual LetterType LetterType { get; set; } = null!;
}
