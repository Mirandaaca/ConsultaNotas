using ConsultaNotas.Entities;
using ConsultaNotas.Interfaces;
using ConsultaNotas.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsultaNotas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotasPinController : ControllerBase
    {
        private readonly INotasPinRepository _notasPinRepository;
        public NotasPinController(INotasPinRepository notasPinRepository)
        {
            _notasPinRepository = notasPinRepository;
        }
        [HttpGet("ObtenerPinDeEstudiante")]
        public async Task<ActionResult<NotasPin>> ObtenerPinDeEstudiante(int registro)
        {
            try
            {
                var response = await _notasPinRepository.ObtenerPinEstudiante(registro);
                return Ok(new Response<NotasPin>(message: "Pin obtenido correctamente", data: response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>(message: ex.Message, succeded: false));
            }
        }
    }
}
