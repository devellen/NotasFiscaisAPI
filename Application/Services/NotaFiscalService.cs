using Application.Interfaces;
using AutoMapper;
using Domain.DTOs;
using Domain.Genericos;
using Infrastructure.Interfaces;

namespace Application.Services
{
    public class NotaFiscalService : INotaFiscalService
    {
        private readonly INotaFiscalRepository _repository;
        private readonly IMapper _mapper;

        public NotaFiscalService(INotaFiscalRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> ExcluirDocumento(int id)
        {
            try
            {
                return await _repository.ExcluirDocumento(id);
            }
            catch (Exception) { throw; }
        }

        public async Task<ResultadoPaginado<DocFiscalDto>> ListarDocumentos(string? filtro, int pagina, int tamanhoPagina)
        {
            var documentos = await _repository.ListarDocumentos(filtro, pagina, tamanhoPagina);

            return new ResultadoPaginado<DocFiscalDto>
            {
                Itens = _mapper.Map<List<DocFiscalDto>>(documentos.Item1),
                ContagemTotal = documentos.Item2
            };
        }

        public async Task<DocFiscalDto> ObterDocumentoPorId(int id)
        {
            try
            {
                var doc = await _repository.ObterDocumentoPorId(id);
                return _mapper.Map<DocFiscalDto>(doc);
            }
            catch (Exception) { throw; }
        }
    }
}
