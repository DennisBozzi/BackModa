using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace Back.Models;

public class Venda
{
    [Key] public int Id { get; set; }
    public double ValorTotal { get; set; }
    public DateTime VendidoEm { get; set; } = DateTime.Now;
}