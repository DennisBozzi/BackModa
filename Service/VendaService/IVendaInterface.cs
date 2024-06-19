using Back.Models;
using Back.Models.Dto;

namespace Back.Service.VendaService;

public interface IVendaInterface
{
    Task<ServiceResponse<List<Venda>>> GetVendas();
    Task<ServiceResponse<List<Venda>>> DeleteVenda(int id);
    Task<ServiceResponse<List<Venda>>> EfetuarVenda(List<VendaDto> vendaDto);
}