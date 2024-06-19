using Back.Models;
using Back.Models.Dto;

namespace Back.Service.ProdutoService;

public interface IProdutoInterface
{
    Task<ServiceResponse<List<Produto>>> GetProdutos();
    Task<ServiceResponse<List<Produto>>> CreateProduto(CreateProdutoDto cpDto);
    Task<ServiceResponse<Produto>> GetProdutoById(int id);
    Task<ServiceResponse<List<Produto>>> DeleteProduto(int id);
}