using Back.Models;
using Back.Models.Dto;

namespace Back.Services.ProdutoService;

public interface IProdutoInterface
{
    Task<ServiceResponse<PaginationHelper<Produto>>> GetProdutos(ProdutoFiltro filtro);
    Task<ServiceResponse<Produto>> GetProdutoById(int id);
    Task<ServiceResponse<List<Produto>>> GetProdutoNaoVendido();
    Task<ServiceResponse<List<Produto>>> CreateProduto(ProdutoDto produtoDto);
    Task<ServiceResponse<List<Produto>>> DeleteProduto(int id);
    Task<ServiceResponse<List<Produto>>> UpdateProduto(ProdutoDto produto);
}