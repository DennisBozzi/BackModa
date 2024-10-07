using Back.Context;
using Back.Enum;
using Back.Models;
using Back.Models.Dto;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;

namespace Back.Services.ProdutoService;

public class ProdutoService : IProdutoInterface
{
    private readonly AppDbContext _context;

    public ProdutoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<PaginationHelper<Produto>>> GetProdutos(ProdutoFiltro filtro)
    {
        ServiceResponse<PaginationHelper<Produto>> response = new();
        PaginationHelper<Produto> pagination = new();
        IQueryable<Produto> data;
        var tipoProduto = filtro.TipoProduto ?? TipoProduto.Todos;
        var isVendido = tipoProduto == TipoProduto.Vendidos ? true : false;

        try
        {
            data = _context.Produtos;
            
            if (tipoProduto != TipoProduto.Todos)
                data = data.Where(x => x.Vendido == isVendido);
            if (filtro.Nome != null)
                data = data.Where(x => x.Nome.ToLower().Contains(filtro.Nome.ToLower()));

            pagination.Data = data.OrderBy(x => x.Id).ToList();
            pagination.PageNumber = filtro.PageNumber;
            pagination.PageSize = filtro.PageSize;
            pagination.Formater();
            response.Objeto = pagination;
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

    public async Task<ServiceResponse<List<Produto>>> GetProdutosNaoVendido()
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

    public async Task<ServiceResponse<Produto>> CreateProduto(ProdutoDto produto)
    {
        ServiceResponse<Produto> response = new();

        try
        {
            Validar(produto);

            Produto novoProduto = new Produto { Nome = produto.Nome, Preco = produto.Preco };

            _context.Produtos.Add(novoProduto);
            await _context.SaveChangesAsync();
            response.Objeto = novoProduto;
            response.Mensagem = $"Produto {novoProduto.Nome} criado com sucesso!";
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
        }

        return response;
    }

    public async Task<ServiceResponse<Produto>> DeleteProduto(int id)
    {
        ServiceResponse<Produto> response = new();

        Produto produto = _context.Produtos.FirstOrDefault(x => x.Id == id);

        try
        {
            if (produto.Vendido)
                throw new Exception("Produto não pode ser apagado pois já foi vendido!");

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            response.Objeto = produto;
            response.Mensagem = $"Produto de id {id} e nome {produto.Nome}, deletado com sucesso!";
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
        }

        return response;
    }

    public async Task<ServiceResponse<Produto>> UpdateProduto(ProdutoDto newProduto)
    {
        ServiceResponse<Produto> response = new();

        try
        {
            var produto = _context.Produtos.FirstOrDefault(x => x.Id == newProduto.Id);

            Validar(newProduto);

            produto.Nome = newProduto.Nome;
            produto.Preco = newProduto.Preco;

            _context.Produtos.Update(produto);
            _context.SaveChanges();

            response.Objeto = produto;
            response.Mensagem = $"Produto de id {produto.Id} e nome {produto.Nome}, atualizado com sucesso!";
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
        }

        return response;
    }

    private void Validar(ProdutoDto produto)
    {
        if (produto.Nome == "" || produto.Nome == null)
        {
            throw new Exception("Nome do produto é obrigatório!");
        }

        if (produto.Preco == 0 || produto.Preco == null)
        {
            throw new Exception("Preço do produto é obrigatório!");
        }
    }
}