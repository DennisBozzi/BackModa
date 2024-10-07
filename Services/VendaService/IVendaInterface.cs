using Back.Models;
using Back.Models.Dto;

namespace Back.Services.VendaService;

public interface IVendaInterface
{
    Task<ServiceResponse<PaginationHelper<Venda>>> GetVendas(VendaFiltro filtro);
    Task<ServiceResponse<Venda>> GetVendaById(int id);
    Task<ServiceResponse<Venda>> DeleteVenda(int id);
    Task<ServiceResponse<Venda>>DeleteProdutoVenda(int idProduto);
    Task<ServiceResponse<Venda>> EfetuarVenda(VendaDto vendaDto);
}