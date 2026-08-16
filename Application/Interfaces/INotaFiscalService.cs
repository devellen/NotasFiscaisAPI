using Domain.DTOs;

namespace Application.Interfaces
{
    public interface INotaFiscalService
    {
        Task<DocFiscalDto> ObterDocumentoPorId(int id);
        Task<bool> ExcluirDocumento(int id);
    }
}
