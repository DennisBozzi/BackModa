using Back.Context;
using Back.Models;
using Back.Models.Dto;
using Back.Services.VendaService;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;

namespace Back.Services.ProdutoService;

public class VendaService : IVendaInterface
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    private IVendaInterface _vendaInterfaceImplementation;

    public VendaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<PaginationHelper<Venda>>> GetVendas(int pageNumber, int pageSize)
    {
        ServiceResponse<PaginationHelper<Venda>> response = new();
        PaginationHelper<Venda> pagination = new();
        var Query = _context.Vendas.Include(x => x.Produtos).AsQueryable();
        try
        {
            pagination.Data = Query.ToList();
            pagination.PageNumber = pageNumber;
            pagination.PageSize = pageSize;
            pagination.Formater();

            response.Objeto = pagination;
            response.Mensagem = "Vendas retornadas com sucesso!";
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
        }

        return response;
    }

    public async Task<ServiceResponse<Venda>> GetVendaById(int id)
    {
        ServiceResponse<Venda> response = new();

        try
        {
            Venda venda = _context.Vendas.Include(x => x.Produtos).FirstOrDefault(x => x.Id == id);
            response.Objeto = venda;
            response.Mensagem = venda == null ? "Venda não encontrada!" : "Venda retornada com sucesso!";
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
        }

        return response;
    }

    public async Task<ServiceResponse<Venda>> DeleteVenda(int id)
    {
        ServiceResponse<Venda> response = new();

        Venda venda = _context.Vendas.Include(x => x.Produtos).FirstOrDefault(x => x.Id == id);
        List<Produto> produtos = venda.Produtos.ToList();

        try
        {
            foreach (var produto in produtos)
            {
                produto.Vendido = false;
                produto.Venda = null;
            }

            _context.Vendas.Remove(venda);
            await _context.SaveChangesAsync();

            response.Mensagem = "Venda deletada com sucesso!";
            response.Objeto = venda;

            return response;
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
            return response;
        }
    }

    public async Task<ServiceResponse<Venda>> DeleteProdutoVenda(int idProduto)
    {
        ServiceResponse<Venda> response = new();
        Produto produto = _context.Produtos.FirstOrDefault(x => x.Id == idProduto);
        Venda venda = _context.Vendas.Include(x => x.Produtos).FirstOrDefault(x => x.Produtos.Contains(produto));

        try
        {
            if (produto == null)
                throw new Exception("Produto não encontrado.");

            venda.Produtos.Remove(produto);
            venda.ValorTotal -= produto.Preco;

            if (venda.Produtos.Count <= 1)
                _context.Vendas.Remove(venda);

            produto.Vendido = false;

            await _context.SaveChangesAsync();

            response.Mensagem = $"Produto: {produto.Nome}, removido da venda com sucesso!";
            return response;
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
            return response;
        }
    }

    public async Task<ServiceResponse<Venda>> EfetuarVenda(VendaDto vendaDto)
    {
        ServiceResponse<Venda> response = new();
        var produtos = vendaDto.Produtos;
        Venda venda = new Venda();

        using (var transaction = await _context.Database.BeginTransactionAsync())
        {
            try
            {
                venda.Desconto = vendaDto.Desconto ?? 0;
                _context.Vendas.Add(venda);
                await _context.SaveChangesAsync();

                foreach (var id in produtos)
                {
                    var produto = _context.Produtos.FirstOrDefault(x => x.Id == id);

                    if (produto.Vendido)
                        throw new Exception("Produto " + produto.Nome + " já está vendido.");

                    if (produto != null)
                    {
                        produto.Vendido = true;
                        venda.Produtos.Add(produto);
                        venda.ValorTotal += produto.Preco;
                    }
                }

                venda.ValorTotal -= venda.Desconto;
                venda.Desconto = Math.Round(venda.Desconto, 2);
                venda.ValorTotal = Math.Round(venda.ValorTotal, 2);

                if (produtos.Count == 0)
                    throw new Exception("Nenhum produto selecionado.");

                if (venda.ValorTotal <= vendaDto.Desconto)
                    throw new Exception("Desconto maior que o valor total da compra.");

                _context.Vendas.Update(venda);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                response.Objeto = venda;
                response.Mensagem = "Venda efetuada com sucesso!";
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                response.Mensagem = e.Message;
                response.Successo = false;
            }
        }

        return response;
    }
}