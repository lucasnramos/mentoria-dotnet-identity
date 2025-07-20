using System;
using System.ComponentModel.DataAnnotations;

namespace Identity.Application.AppUser.Input;

public class UserInput
{
    public required string Name { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [Required]
    public required string Email { get; set; }

    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    [MaxLength(100, ErrorMessage = "Password cannot exceed 100 characters.")]
    [DataType(DataType.Password)]
    [Required]
    public required string Password { get; set; }
    public required string Role { get; set; }
}
