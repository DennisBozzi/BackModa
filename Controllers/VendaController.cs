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

    [HttpGet]
    public async Task<ServiceResponse<List<Venda>>> GetVendas()
    {
        return await _vendaInterface.GetVendas();
    }
    
    [HttpGet("{id}")]
    public async Task<ServiceResponse<Venda>> GetVendaById(int id)
    {
        return await _vendaInterface.GetVendaById(id);
    }
    
    
    [HttpPost("EfetuarVenda")]
    public async Task<ServiceResponse<Venda>> EfetuarVenda(VendaDto vendaDto)
    {
        return await _vendaInterface.EfetuarVenda(vendaDto);
    }

    [HttpDelete("DeleteProdutoVenda/{idProduto}")]
    public async Task<ServiceResponse<Venda>> DeleteProdutoVenda(int idProduto){
        return await _vendaInterface.DeleProdutoVenda(idProduto);
    }
    
    [HttpDelete("{id}")]
    public async Task<ServiceResponse<Venda>> DeleteVenda(int id)
    {
        return await _vendaInterface.DeleteVenda(id);
    }
}