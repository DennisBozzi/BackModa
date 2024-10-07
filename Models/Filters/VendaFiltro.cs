namespace Back.Models.Dto;

public class VendaFiltro
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? NomeProduto { get; set; }
}