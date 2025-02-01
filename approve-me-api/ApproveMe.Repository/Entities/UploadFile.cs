using System;
using System.Collections.Generic;

namespace ApproveMe.Repository.Entities;

public partial class UploadFile
{
    public long Id { get; set; }

    public string? Code { get; set; }

    public string FileName { get; set; } = null!;

    public string Path { get; set; } = null!;

    public decimal Size { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual ICollection<LetterAttachment> LetterAttachments { get; set; } = new List<LetterAttachment>();
}
