

namespace Back.Models;

public class ServiceResponse<T>
{
    public T? Objeto { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public bool Successo { get; set; } = true;
}