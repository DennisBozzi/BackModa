using Back.Models;
using Back.Service.ValorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers;

[ApiController]
[Route("[controller]")]
public class ValorController : ControllerBase
{
    private readonly IValorInterface _valorInterface;

    public ValorController(IValorInterface valorInterface)
    {
        _valorInterface = valorInterface;
    }

    [HttpGet]
    public async Task<ServiceResponse<List<Valor>>> GetValor()
    {
        return await _valorInterface.GetValor();
    }

    [HttpPost]
    public async Task<ServiceResponse<List<Valor>>> CreateValor(double preco)
    {
        return await _valorInterface.CreateValor(preco);
    }

    [HttpDelete]
    public async Task<ServiceResponse<List<Valor>>> DeleteValor(int id)
    {
        return await _valorInterface.DeleteValor(id);
    }
}