using Domain.Models;

namespace Infrastructure.Interfaces
{
    public interface INotaFiscalRepository
    {
        Task<DocFiscal> ObterDocumentoPorId(int id);
        Task<bool> AtualizarDocumento(DocFiscal documento);
        Task<int> InserirDocumento(DocFiscal documento);
        Task<(IEnumerable<DocFiscal>, int)> ListarDocumentos(string? filtro, int pagina, int tamanhoPagina);
        Task<bool> ExcluirDocumento(int id);
    }
}
