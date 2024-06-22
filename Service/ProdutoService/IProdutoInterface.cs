﻿using Back.Models;
using Back.Models.Dto;

namespace Back.Service.ProdutoService;

public interface IProdutoInterface
{
    Task<ServiceResponse<List<Produto>>> GetProdutos();
    Task<ServiceResponse<Produto>> GetProdutoById(int id);
    Task<ServiceResponse<List<Produto>>> GetProdutoNotVendido();
    Task<ServiceResponse<List<Produto>>> CreateProduto(ProdutoDto produtoDto);
    Task<ServiceResponse<List<Produto>>> DeleteProduto(int id);
}