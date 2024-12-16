using System;

namespace API.DTOs;

public class MemberDto
{
    // define properties which to be returned when user request for list of users
     public int Id { get; set; }
    public  string? UserName { get; set; }
    
    public int Age { get; set; }
    public string? PhotoUrl { get; set; }

    public string? knownAs { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastActive { get; set; }
    public  string? Introuction  { get; set; }
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }

    public string?  City { get; set; }
    public  string? Country { get; set; }
    public List<PhotoDto>? Photos { get; set; }

}