using System;
using System.Collections.Generic;

namespace ApproveMe.Repository.Entities;

public partial class LetterAttachment
{
    public long Id { get; set; }

    public long LetterId { get; set; }

    public long UploadFileId { get; set; }

    public bool? IsPrimary { get; set; }

    public long CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public long? ModifiedBy { get; set; }

    public DateTime? ModifiedAt { get; set; }

    public virtual Letter Letter { get; set; } = null!;

    public virtual UploadFile UploadFile { get; set; } = null!;
}
