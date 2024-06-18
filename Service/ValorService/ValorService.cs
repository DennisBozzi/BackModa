using Back.Context;
using Back.Models;
using FirebaseAdmin.Auth;

namespace Back.Service.ValorService;

public class ValorService : IValorInterface
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;


    public ValorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceResponse<List<Valor>>> GetValor()
    {
        ServiceResponse<List<Valor>> response = new ServiceResponse<List<Valor>>();

        try
        {
            response.Objeto = _context.Valores.ToList();
            response.Mensagem = "Valores retornados com sucesso!";
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
        }

        return response;
    }

    public async Task<ServiceResponse<List<Valor>>> CreateValor(double preco)
    {
        ServiceResponse<List<Valor>> response = new ServiceResponse<List<Valor>>();

        Valor newValor = new Valor { Preco = preco };

        try
        {
            _context.Valores.Add(newValor);

            await _context.SaveChangesAsync();

            response.Objeto = _context.Valores.ToList();
            response.Mensagem = "Valor " + preco + " criado com sucesso!";
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
        }

        return response;
    }

    public async Task<ServiceResponse<List<Valor>>> DeleteValor(int id)
    {
        ServiceResponse<List<Valor>> response = new ServiceResponse<List<Valor>>();

        try
        {
            Valor valor = _context.Valores.FirstOrDefault(x => x.Id == id);

            if (valor == null)
            {
                throw new Exception("Valor não encontrado!");
            }

            _context.Remove(valor);

            await _context.SaveChangesAsync();

            response.Objeto = _context.Valores.ToList();

            response.Mensagem = "Valor " + valor.Preco + " deletado com sucesso!";
        }
        catch (Exception e)
        {
            response.Mensagem = e.Message;
            response.Successo = false;
        }


        return response;
    }
}