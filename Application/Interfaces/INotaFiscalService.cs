using Domain.DTOs;

namespace Application.Interfaces
{
    public interface INotaFiscalService
    {
        Task<DocFiscalDto> ObterDocumentoPorId(int id);
    }
}
