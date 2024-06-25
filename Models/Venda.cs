using System.ComponentModel.DataAnnotations;
using System.Text.Json.Nodes;

namespace Back.Models;

public class Venda
{
    [Key] public int Id { get; set; }
    public double ValorTotal { get; set; } = 0;
    public double Desconto { get; set; } = 0;
    public DateTime VendidoEm { get; set; } = DateTime.Now.ToUniversalTime();
    public ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}