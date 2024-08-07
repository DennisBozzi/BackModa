using Back.Enum;

namespace Back.Models.Dto;

public class ProdutoFiltro
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? Nome { get; set; }
    public TipoProduto TipoProduto { get; set; }
}