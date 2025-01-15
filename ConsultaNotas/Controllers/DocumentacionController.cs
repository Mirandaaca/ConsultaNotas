using ConsultaNotas.Entities;
using ConsultaNotas.Interfaces;
using ConsultaNotas.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsultaNotas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentacionController : ControllerBase
    {
        private readonly IDocumentacionRepository _documentacionRepository;
        public DocumentacionController(IDocumentacionRepository documentacionRepository)
        {
            _documentacionRepository = documentacionRepository;
        }
        [HttpGet("ObtenerDocumentosDeEstudiante")]
        public async Task<ActionResult<Documentos>> ObtenerDocumentosDeEstudiante(int registro)
        {
            try
            {
                var response = await _documentacionRepository.ObtenerDocumentosDeEstudiante(registro);
                return Ok(new Response<Documentos>(message: "Documentos obtenidos correctamente", data: response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>(message: ex.Message, succeded: false));
            }
        }
    }
}
