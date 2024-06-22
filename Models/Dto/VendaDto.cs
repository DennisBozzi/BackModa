namespace Back.Models.Dto;

public class VendaDto
{
    public List<int> Produtos { get; set; }
    public double Desconto { get; set; } = 0;
}