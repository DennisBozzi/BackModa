using Back.Models;
using Back.Models.Dto;

namespace Back.Service.VendaService;

public interface IVendaInterface
{
    Task<ServiceResponse<List<Venda>>> GetVendas();
    Task<ServiceResponse<Venda>> GetVendaById(int id);
    Task<ServiceResponse<Venda>> DeleteVenda(int id);
    Task<ServiceResponse<Venda>>DeleProdutoVenda(int idProduto);
    Task<ServiceResponse<Venda>> EfetuarVenda(VendaDto vendaDto);
}