using AutoMapper;
using Ecommerce.Api.Controllers.Empresa.Dto;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Ecommerce.Api.Controllers.Empresa
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EmpresaController : ControllerBase
    {
        private readonly IEmpresaServices _empresaService;
        private readonly IMapper _mapper;

        private const string MensagemEmpresaNaoEncontrada = "Empresa não encontrada.";

        public EmpresaController(IEmpresaServices empresaService, IMapper mapper)
        {
            _empresaService = empresaService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ObterEmpresaPeloCnpjResponse>> ObterEmpresaPeloCnpj([FromQuery(Name = "request")] string request)
        {
            if (request is null)
                return BadRequest();

            var result = await _empresaService.GetEmpresaByCnpj(request);

            if (result is null)
                return UnprocessableEntity(MensagemEmpresaNaoEncontrada);

            var response = _mapper.Map<EmpresaModel, ObterEmpresaPeloCnpjResponse>(result);

            return Ok(response);
        }
    }
}
