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

        public async Task<bool> ExcluirDocumento(int id)
        {
            try
            {
                var sql = $"DELETE FROM DocumentoFiscal WHERE Id = {id}";
                var res = await _connection.ExecuteAsync(sql);
                return res > 0 ? true : false;
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<(IEnumerable<DocFiscal>, int)> ListarDocumentos(string? filtro, int pagina, int tamanhoPagina)
        {
            int totalCount;
            var sql = @"SELECT * FROM DocumentoFiscal ";

            var condicao = @"
                            WHERE 
                            CnpjEmitente LIKE '%' + @FILTRO + '%'
                            OR Uf LIKE '%' + @FILTRO + '%'
                            OR RazaoSocialEmitente  LIKE '%' + @FILTRO + '%'
                        ";


            if (!string.IsNullOrEmpty(filtro))
            {
                sql += condicao;
                totalCount = await _connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM DocumentoFiscal {condicao}", new { FILTRO = filtro });
            }
            else
            {
                totalCount = await _connection.ExecuteScalarAsync<int>($"SELECT COUNT(*) FROM DocumentoFiscal");
            }

            sql += $" ORDER BY DataEmissao OFFSET {(pagina - 1) * tamanhoPagina} ROWS FETCH NEXT {tamanhoPagina} ROWS ONLY";

            var resultado = await _connection.QueryAsync<DocFiscal>(sql, new { FILTRO = filtro });

            return (resultado.ToList(), totalCount);
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
