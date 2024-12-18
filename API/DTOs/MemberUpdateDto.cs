using System;

namespace API.DTOs;

public class MemberUpdateDto
{
    //get the properties from client's side
    public string?  Introduction { get; set; }
    public string? LookingFro { get; set; }
    public string? Interests { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }

}
