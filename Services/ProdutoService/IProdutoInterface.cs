using Back.Models;
using Back.Models.Dto;

namespace Back.Services.ProdutoService;

public interface IProdutoInterface
{

    Task<ServiceResponse<PaginationHelper<Produto>>> GetProdutos(ProdutoFiltro filtro);
    Task<ServiceResponse<Produto>> GetProdutoById(int id);
    Task<ServiceResponse<List<Produto>>> GetProdutosNaoVendido();
    Task<ServiceResponse<Produto>> CreateProduto(ProdutoDto produtoDto);
    Task<ServiceResponse<Produto>> DeleteProduto(int id);
    Task<ServiceResponse<Produto>> UpdateProduto(ProdutoDto produto);
}