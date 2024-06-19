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
            response.Objeto = _context.Produtos.Include(x => x.Valor).ToList();
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

        Produto produto = _context.Produtos.Include(x => x.Valor).FirstOrDefault(x => x.Id == id);

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

    public async Task<ServiceResponse<List<Produto>>> CreateProduto(CreateProdutoDto cpDto)
    {
        ServiceResponse<List<Produto>> response = new();

        Produto produto = _context.Produtos.Include(x => x.Valor).FirstOrDefault(x => x.Valor.Id == cpDto.valorId);

        string msg;

        try
        {
            if (produto != null)
            {
                produto.Quantidade += cpDto.quantidade;
                _context.Produtos.Update(produto);

                msg = $"Ao produto de valor {produto.Valor.Preco}, foi adicionado {cpDto.quantidade} unidades!";
            }
            else
            {
                produto = new Produto
                {
                    Valor = _context.Valores.FirstOrDefault(x => x.Id == cpDto.valorId),
                    Quantidade = cpDto.quantidade
                };
                _context.Produtos.Add(produto);

                msg = $"Foi criado um novo produto de valor: {produto.Valor.Preco} com {cpDto.quantidade} unidades!";
            }

            await _context.SaveChangesAsync();
            response.Objeto = _context.Produtos.ToList();
            response.Mensagem = msg;
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
            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            response.Objeto = _context.Produtos.ToList();
            response.Mensagem = $"Produto de id {id} e valor {produto.Valor} deletado com sucesso!";
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
        }

        return response;
    }
}