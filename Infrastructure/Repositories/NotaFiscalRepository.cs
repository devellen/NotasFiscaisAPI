using System.Data;
using Dapper;
using Domain.Models;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class NotaFiscalRepository : INotaFiscalRepository
    {
        private readonly IDbConnection _connection;

        public NotaFiscalRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<DocFiscal> ObterDocumentoPorId(int id)
        {
            try
            {
                var sql = $"SELECT * FROM DocumentoFiscal WHERE Id = {id}";
                var documento = await _connection.QueryFirstOrDefaultAsync<DocFiscal>(sql);
                return documento;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
