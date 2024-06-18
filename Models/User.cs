using System.ComponentModel.DataAnnotations;

namespace Back.Models;

public class User
{
    [Key] public int Id { get; set; }
    public string IdFirebase { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool Admin { get; set; } = false;
}