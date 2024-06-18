using Back.Models;

namespace Back.Service.ValorService;

public interface IValorInterface
{
    Task<ServiceResponse<List<Valor>>> GetValor();
    Task<ServiceResponse<List<Valor>>> CreateValor(double preco);
    Task<ServiceResponse<List<Valor>>> DeleteValor(int id);
}