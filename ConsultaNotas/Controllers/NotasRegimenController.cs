using ConsultaNotas.Entities;
using ConsultaNotas.Interfaces;
using ConsultaNotas.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsultaNotas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotasRegimenController : ControllerBase
    {
        private readonly INotasRegimenRepository _notasRegimenRepository;
        public NotasRegimenController(INotasRegimenRepository notasRegimenRepository)
        {
            _notasRegimenRepository = notasRegimenRepository;
        }
        [HttpGet("ObtenerInformacionSemestre")]
        public async Task<ActionResult<NotasRegimen>> ObtenerInformacionSemestre(int regimen)
        {
            try
            {
                var response = await _notasRegimenRepository.ObtenerInformacionSemestre(regimen);
                return Ok(new Response<NotasRegimen>(message: "Informacion obtenida correctamente", data: response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>(message: ex.Message, succeded: false));
            }
        }
    }
}
