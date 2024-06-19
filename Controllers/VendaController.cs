using Back.Models;
using Back.Models.Dto;
using Back.Service.ProdutoService;
using Back.Service.VendaService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers;

[ApiController]
[Route("[controller]")]
public class VendaController : ControllerBase
{
    private readonly IVendaInterface _vendaInterface;

    public VendaController(IVendaInterface vendaInterface)
    {
        _vendaInterface = vendaInterface;
    }

    [HttpPost("EfetuarVenda")]
    public async Task<ServiceResponse<List<Venda>>> EfetuarVenda(List<VendaDto> vendaDto)
    {
        return await _vendaInterface.EfetuarVenda(vendaDto);
    }
}