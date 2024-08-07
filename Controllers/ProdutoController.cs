using Back.Enum;
using Back.Models;
using Back.Models.Dto;
using Back.Services.ProdutoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoInterface _produtoInterface;

    public ProdutoController(IProdutoInterface produtoInterface)
    {
        _produtoInterface = produtoInterface;
    }

    [HttpGet]
    public async Task<ServiceResponse<PaginationHelper<Produto>>> GetProdutos([FromQuery] ProdutoFiltro filtro)
    {
        return await _produtoInterface.GetProdutos(filtro);
    }

    [HttpGet("{id}")]
    public async Task<ServiceResponse<Produto>> GetProdutoById(int id)
    {
        return await _produtoInterface.GetProdutoById(id);
    }

    [HttpGet("GetProdutoNaoVendido")]
    public async Task<ServiceResponse<List<Produto>>> GetProdutoNaoVendido()
    {
        return await _produtoInterface.GetProdutoNaoVendido();
    }

    [HttpPut]
    public async Task<ServiceResponse<List<Produto>>> UpdateProduto(ProdutoDto produto)
    {
        return await _produtoInterface.UpdateProduto(produto);
    }

    [HttpPost]
    public async Task<ServiceResponse<List<Produto>>> CreateValor(ProdutoDto produtoDto)
    {
        return await _produtoInterface.CreateProduto(produtoDto);
    }

    [HttpDelete("{id}")]
    public async Task<ServiceResponse<List<Produto>>> DeleteProduto(int id)
    {
        return await _produtoInterface.DeleteProduto(id);
    }
}