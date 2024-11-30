using System;
using System.ComponentModel.DataAnnotations;

namespace API.Data;

public class RegisterDto
{
    //taking objects from client side's and sending it to DTO which converts them from objects to parameters & then send it to controller
    [Required]
    public required string UserName { get; set; }
    [Required]
    public required string Password { get; set; }

}
