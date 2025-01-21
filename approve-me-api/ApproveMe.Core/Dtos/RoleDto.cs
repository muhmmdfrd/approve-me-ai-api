using ApproveMe.Core.Enums;
using Flozacode.Models.Paginations;
using Newtonsoft.Json;

namespace ApproveMe.Core.Dtos;

public class RoleDto
{
    public string Name { get; set; } = null!;
    
    [JsonIgnore]
    public int DataStatusId { get; set; } = (int)DataStatusEnum.Active;
}

public class RoleViewDto : RoleDto
{
    public long Id { get; set; }
}

public class RoleAddDto : RoleDto
{
    [JsonIgnore]
    public long? CreatedBy { get; set; }

    [JsonIgnore]
    public DateTime? CreatedAt { get; set; }

    [JsonIgnore]
    public long? ModifiedBy { get; set; }

    [JsonIgnore]
    public DateTime? ModifiedAt { get; set; }
}

public class RoleUpdDto : RoleDto
{
    public long Id { get; set; }
    
    [JsonIgnore]
    public long? ModifiedBy { get; set; }

    [JsonIgnore]
    public DateTime? ModifiedAt { get; set; }
}

public class RoleFilter : TableFilter
{
}