using Back.Context;
using Back.Models;
using Back.Models.Dto;
using Back.Service.VendaService;
using FirebaseAdmin.Auth;

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
            response.Objeto = _context.Vendas.ToList();
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
        //TODO: Implementar método de deletar venda após deletar todas as VendasProdutos referenciadas.
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<List<Venda>>> EfetuarVenda(List<VendaDto> vendaDto)
    {
        //TODO: Implementar venda. Primeiro criar uma Venda e depois referenciar as VendasProdutos de acordo com a lista.

        ServiceResponse<List<Venda>> response = new();

        // Produto produto = _context.Produtos.FirstOrDefault(x => x.Id == vendaDto.ProdutoId);
        // Venda venda = _context.Vendas.FirstOrDefault(x => x.Id == vendaId);

        try
        {
            await _context.SaveChangesAsync();

            response.Objeto = _context.Vendas.ToList();
            response.Mensagem = $"Venda efetuada com sucesso!";
        }
        catch (Exception e)
        {
            await _context.DisposeAsync();
            response.Mensagem = e.Message;
            response.Successo = false;
        }

        return response;
    }
}