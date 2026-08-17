using Application.Interfaces;
using Domain.DTOs;
using Domain.Genericos;
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
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
                return BadRequest("Arquivo não informado.");

            using var reader = new StreamReader(arquivo.OpenReadStream());

            var xml = await reader.ReadToEndAsync();

            var documento = await _service.ProcessarXml(xml);

            return Ok(documento);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarDocumento(int id, [FromBody] DocFiscalDto doc)
        {
            try
            {
                var res = await _service.AtualizarDocumento(id, doc);
                if (res) return Ok("Documento atualizado com sucesso");
                return BadRequest("Erro ao tentar atualizar documento");
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
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
        [HttpGet]
        public async Task<ActionResult<ResultadoPaginado<DocFiscalDto>>> ObterTodos([FromQuery] string? filtro, [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 50)
        {
            var documentos = await _service.ListarDocumentos(filtro, pagina, tamanhoPagina);
            return Ok(documentos);
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
