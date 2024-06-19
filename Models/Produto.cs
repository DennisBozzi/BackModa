using System.ComponentModel.DataAnnotations;

namespace Back.Models;

public class Produto
{
    [Key] public int Id { get; set; }
    public int Quantidade { get; set; }
    public Valor Valor { get; set; }
}