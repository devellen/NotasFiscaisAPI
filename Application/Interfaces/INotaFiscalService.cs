using Domain.DTOs;
using Domain.Genericos;

namespace Application.Interfaces
{
    public interface INotaFiscalService
    {
        Task<bool> AtualizarDocumento(int id, DocFiscalDto documento);
        Task<DocFiscalDto> ObterDocumentoPorId(int id);
        Task<ResultadoPaginado<DocFiscalDto>> ListarDocumentos(string? filtro, int pagina, int tamanhoPagina);
        Task<bool> ExcluirDocumento(int id);
    }
}
