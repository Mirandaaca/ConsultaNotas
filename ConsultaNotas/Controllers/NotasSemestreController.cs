using ConsultaNotas.DTOs.Semestre;
using ConsultaNotas.Entities;
using ConsultaNotas.Interfaces;
using ConsultaNotas.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsultaNotas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotasSemestreController : ControllerBase
    {
        private readonly INotasSemestreRepository _notasSemestreRepository;
        public NotasSemestreController(INotasSemestreRepository notasSemestreRepository)
        {
            _notasSemestreRepository = notasSemestreRepository;
        }
        [HttpGet("ObtenerNotasDeUnSemestre")]
        public async Task<ActionResult<NotasYPeriodoDeUnSemestreDTO>> ObtenerNotasDeUnSemestre(int registro, int ano, int semestre)
        {
            try
            {
                var response = await _notasSemestreRepository.ObtenerNotasDeUnSemestre(registro, ano, semestre);
                return Ok(new Response<NotasYPeriodoDeUnSemestreDTO>(message: "Notas obtenidas correctamente", data: response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>(message: ex.Message, succeded: false));
            }
        }
    }
}
