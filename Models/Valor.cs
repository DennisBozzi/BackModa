using System.ComponentModel.DataAnnotations;

namespace Back.Models;

public class Valor
{
    [Key] public int Id { get; set; }
    public double Preco { get; set; }
}