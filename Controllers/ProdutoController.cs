﻿using Back.Models;
using Back.Models.Dto;
using Back.Service.ProdutoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Back.Controllers;

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
    public async Task<ServiceResponse<List<Produto>>> GetValor()
    {
        return await _produtoInterface.GetProdutos();
    }

    [HttpGet("GetProdutoById")]
    public async Task<ServiceResponse<Produto>> GetProdutoById(int id)
    {
        return await _produtoInterface.GetProdutoById(id);
    }

    [HttpPost]
    public async Task<ServiceResponse<List<Produto>>> CreateValor(CreateProdutoDto cpDto)
    {
        return await _produtoInterface.CreateProduto(cpDto);
    }

    [HttpDelete]
    public async Task<ServiceResponse<List<Produto>>> DeleteProduto(int id)
    {
        return await _produtoInterface.DeleteProduto(id);
    }
}