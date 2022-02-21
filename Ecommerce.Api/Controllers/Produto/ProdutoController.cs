using AutoMapper;
using Ecommerce.Api.Controllers.Produto.Dto;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Ecommerce.Api.Controllers.Produto
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoCorServices _produtoCorService;
        private readonly IMapper _mapper;

        private const string MensagemProdutoNaoEncontrado = "Produto não encontrado.";

        public ProdutoController(IProdutoCorServices produtoCorServices, IMapper mapper)
        {
            _produtoCorService = produtoCorServices;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<ObterProdutoPeloCodigoBarrasResponse>> ObterProdutoPeloCodigoBarras([FromQuery(Name = "request")] string request)
        {
            if (request is null)
                return BadRequest();

            var result = await _produtoCorService.ObterPeloCodigoBarrasAsync(request);

            if (result is null)
                return UnprocessableEntity(MensagemProdutoNaoEncontrado);

            var response = _mapper.Map<ProdutoCorModel, ObterProdutoPeloCodigoBarrasResponse>(result);

            return Ok(response);
        }
    }
}
