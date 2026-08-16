using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CrudNotasFiscais.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotaFiscalController : ControllerBase
    {
        private readonly INotaFiscalService _service;

        public NotaFiscalController(INotaFiscalService service)
        {
            _service = service;
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> ExcluirDocumento(int id)
        {
            try
            {
                var res = await _service.ExcluirDocumento(id);
                if (res) return Ok("Documento excluído com sucesso");
                return BadRequest("Erro ao tentar excluir documento");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterDocumentoPorId(int id)
        {
            try
            {
                var tarefa = await _service.ObterDocumentoPorId(id);
                if (tarefa == null) return NotFound("documento não encontrado");
                return Ok(tarefa);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

    }
}
