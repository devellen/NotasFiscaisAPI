using Application.Interfaces;
using AutoMapper;
using Domain.DTOs;
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
