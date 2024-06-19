using Back.Context;
using Back.Models;
using FirebaseAdmin.Auth;

namespace Back.Service.ValorService;

public class ValorService : IValorInterface
{
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
            isValorExist(preco);

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

            isValorInProduto(id);

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

    public void isValorExist(double preco)
    {
        if (_context.Valores.FirstOrDefault(x => x.Preco == preco) != null)
        {
            throw new Exception("Este valor já existe!");
        }
    }

    public void isValorInProduto(int id)
    {
        if (_context.Produtos.FirstOrDefault(x => x.Valor.Id == id) != null)
        {
            throw new Exception("Existem produtos vinculados a esse valor!");
        }
    }
}