using System;
using System.ComponentModel.DataAnnotations;

namespace API.Data;

public class RegisterDto
{
    //taking objects from client side's and sending it to DTO which converts them from objects to parameters & then send it to controller
    [Required]
    public string UserName { get; set; } = string.Empty;

    public required string? knownas { get; set; }
    public required string? Gender { get; set; }
    public required string? DateOfBirth { get; set; }
    public required string? City { get; set; }
    public required string? Country { get; set; }    

    [Required]
    [StringLength(8, MinimumLength = 4)]
    public  string Password { get; set; } = string.Empty;

}
