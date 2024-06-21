using System.ComponentModel.DataAnnotations;

namespace Back.Models;

public class Produto
{
    [Key] public int Id { get; set; }
    public string Nome { get; set; }
    public double Preco { get; set; }
    public Venda? Venda { get; set; } = null;
    public bool Vendido { get; set; } = false;
    public DateTime CriadoEm { get; set; } = DateTime.Now;
}