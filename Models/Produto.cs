using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Back.Models;

public class Produto
{
    [Key] public int Id { get; set; }
    public string Nome { get; set; }
    public double Preco { get; set; }
    [JsonIgnore] public Venda? Venda { get; set; }
    public bool Vendido { get; set; } = false;
    public DateTime CriadoEm { get; set; } = DateTime.Now.ToUniversalTime();
}