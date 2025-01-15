using ConsultaNotas.DTOs.Auth;
using ConsultaNotas.Entities;
using ConsultaNotas.Interfaces;
using ConsultaNotas.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsultaNotas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotasEstudianteController : ControllerBase
    {
        private readonly INotasEstudianteRepository _notasEstudianteRepository;
        public NotasEstudianteController(INotasEstudianteRepository notasEstudianteRepository)
        {
            _notasEstudianteRepository = notasEstudianteRepository;
        }
        [HttpGet("ObtenerInformacionBasicaDeEstudiante")]
        public async Task<ActionResult<NotasEstudiante>> ObtenerInformacionBasicaDeEstudiante(int registro)
        {
            try
            {
                var response = await _notasEstudianteRepository.ObtenerInformacionEstudiante(registro);
                return Ok(new Response<NotasEstudiante>(message: "Informacion obtenida correctamente", data: response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>(message: ex.Message, succeded: false));
            }
        }
        [HttpPost("LogIn")]
        public async Task<ActionResult<LogInEstudianteResponseDTO>> LogIn(LogInEstudianteRequestDTO request)
        {
            try
            {
                var response = await _notasEstudianteRepository.LogIn(request);
                return Ok(new Response<LogInEstudianteResponseDTO>(message: "LogIn exitoso", data: response));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Response<bool>(message: ex.Message, succeded: false));
            }
        }
    }
}
