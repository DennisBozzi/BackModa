using Back.Context;
using Back.Models;
using Back.Models.Dto;
using Back.Service.VendaService;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;

namespace Back.Service.ProdutoService;

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

    public async Task<ServiceResponse<List<Venda>>> GetVendas()
    {
        ServiceResponse<List<Venda>> response = new();

        try
        {
            response.Objeto = _context.Vendas.Include(x => x.Produtos).ToList();
            response.Mensagem = "Vendas retornadas com sucesso!";
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
        }

        return response;
    }

    public Task<ServiceResponse<List<Venda>>> DeleteVenda(int id)
    {
        /*
         TODO: Implementar método de deletar venda após editar todos os Produtos referenciadas.
         TODO: Mudar status de vendido e remover a referencia de venda.
         */

        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Venda>> EfetuarVenda(VendaDto vendaDto)
    {
        ServiceResponse<Venda> response = new();
        var produtos = vendaDto.Produtos;
        Venda venda = new Venda();

        try
        {
            venda = _context.Vendas.Add(venda).Entity;
            await _context.SaveChangesAsync();

            foreach (var id in produtos)
            {
                var produto = _context.Produtos.FirstOrDefault(x => x.Id == id);
                if (produto != null)
                {
                    produto.Vendido = true;
                    venda.Produtos.Add(produto);
                    venda.ValorTotal += produto.Preco;
                }
            }

            venda.ValorTotal -= vendaDto.Desconto;

            if (produtos.Count == 0)
                throw new Exception("Nenhum produto selecionado.");

            if (venda.ValorTotal <= vendaDto.Desconto)
                throw new Exception("Desconto maior que o valor total da compra.");

            _context.Vendas.Update(venda);
            await _context.SaveChangesAsync();

            response.Objeto = venda;
            response.Mensagem = $"Venda efetuada com sucesso!";

            //TODO: Ajeitar a resposta para retornar bonito.
        }
        catch (Exception e)
        {
            _context.Vendas.Remove(venda);
            await _context.SaveChangesAsync();

            _context.Dispose();
            await _context.DisposeAsync();

            await _context.DisposeAsync();
            response.Mensagem = e.Message;
            response.Successo = false;
        }

        return response;
    }
}