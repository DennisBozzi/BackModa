using System.ComponentModel.DataAnnotations;

namespace Back.Models;

public class VendaProduto
{
    [Key] public int Id { get; set; }
    public int Quantidade { get; set; }
    public Produto Produto { get; set; }
    public Venda Venda { get; set; }
}