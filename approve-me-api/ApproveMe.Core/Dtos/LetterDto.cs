using System.Text.Json.Serialization;
using Flozacode.Models.Paginations;

namespace ApproveMe.Core.Dtos;

public class LetterDto
{
    public string Title { get; set; } = null!;
    public long LetterTypeId { get; set; }
    public DateTime LetterDate { get; set; }
    public int StatusId { get; set; }
}

public class LetterViewDto : LetterDto
{
    public long CreatorId { get; set; }
    public string CreatorName { get; set; } = null!;
    public long Id { get; set; }
}

public class LetterAddDto : LetterDto
{
    [JsonIgnore]
    public long CreatedBy { get; set; }
    
    [JsonIgnore]
    public DateTime? CreatedAt { get; set; }
    
    [JsonIgnore]
    public long? ModifiedBy { get; set; }
    
    [JsonIgnore]
    public DateTime? ModifiedAt { get; set; }
    
    [JsonIgnore]
    public long CreatorId { get; set; }
}

public class LetterUpdDto : LetterDto
{
    public long Id { get; set; }
    public long CreatorId { get; set; }
    
    [JsonIgnore]
    public long? ModifiedBy { get; set; }
    
    [JsonIgnore]
    public DateTime? ModifiedAt { get; set; }
}

public class LetterFilter : TableFilter
{
    
}