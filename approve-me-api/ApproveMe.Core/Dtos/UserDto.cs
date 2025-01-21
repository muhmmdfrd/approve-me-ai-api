using System.Text.Json.Serialization;
using ApproveMe.Core.Enums;
using Flozacode.Models.Paginations;

namespace ApproveMe.Core.Dtos;

public class UserDto
{
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Code { get; set; }
}

public class UserViewDto : UserDto
{
    public long Id { get; set; }
}

public class UserAddDto : UserDto
{
    public string Password { get; set; } = null!;
    
    [JsonIgnore]
    public long? CreatedBy { get; set; }
    
    [JsonIgnore]
    public DateTime? CreatedAt { get; set; }
    
    [JsonIgnore]
    public long? ModifiedBy { get; set; }
    
    [JsonIgnore]
    public DateTime? ModifiedAt { get; set; }
}

public class UserUpdDto : UserDto
{
    public long Id { get; set; }
    
    [JsonIgnore]
    public long? ModifiedBy { get; set; }
    
    [JsonIgnore]
    public DateTime? ModifiedAt { get; set; }
    public string? File { get; set; }
}

public class UserFilter : TableFilter
{
}