using Back.Context;
using Back.Models;
using Back.Models.Dto;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;

namespace Back.Service.ProdutoService;

public class ProdutoService : IProdutoInterface
{
    private readonly AppDbContext _context;

    public ProdutoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<List<Produto>>> GetProdutos()
    {
        ServiceResponse<List<Produto>> response = new();

        try
        {
            response.Objeto = _context.Produtos.OrderBy(x => x.Id).ToList();
            response.Mensagem = "Produtos retornados com sucesso!";
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
        }

        return response;
    }

    public async Task<ServiceResponse<Produto>> GetProdutoById(int id)
    {
        ServiceResponse<Produto> response = new();

        Produto produto = _context.Produtos.FirstOrDefault(x => x.Id == id);

        try
        {
            if (produto == null)
                throw new Exception("Produto não encontrado!");

            response.Objeto = produto;
            response.Mensagem = "Produtos retornados com sucesso!";
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
        }

        return response;
    }

    public async Task<ServiceResponse<List<Produto>>> GetProdutoNotVendido()
    {
        ServiceResponse<List<Produto>> response = new();

        try
        {
            response.Objeto = _context.Produtos.Where(x => !x.Vendido).ToList();
            response.Mensagem = "Produtos não vendidos, retornados com sucesso!";
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
        }

        return response;
    }

    public async Task<ServiceResponse<List<Produto>>> CreateProduto(ProdutoDto produtoDto)
    {
        ServiceResponse<List<Produto>> response = new();

        Produto produto = new Produto { Nome = produtoDto.Nome, Preco = produtoDto.Preco};

        try
        {
            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();
            response.Objeto = _context.Produtos.OrderBy(x => x.Id).ToList();
            response.Mensagem = $"Produto {produto.Nome} criado com sucesso!";
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
        }

        return response;
    }

    public async Task<ServiceResponse<List<Produto>>> DeleteProduto(int id)
    {
        ServiceResponse<List<Produto>> response = new();

        Produto produto = _context.Produtos.FirstOrDefault(x => x.Id == id);

        try
        {
            if (produto.Vendido)
                throw new Exception("Produto não pode ser apagado pois já foi vendido!");

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            response.Objeto = _context.Produtos.OrderBy(x => x.Id).ToList();
            response.Mensagem = $"Produto de id {id} e nome {produto.Nome}, deletado com sucesso!";
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
        }

        return response;
    }
}