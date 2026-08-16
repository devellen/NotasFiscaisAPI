using Domain.Models;

namespace Infrastructure.Interfaces
{
    public interface INotaFiscalRepository
    {
        Task<DocFiscal> ObterDocumentoPorId(int id);
    }
}
