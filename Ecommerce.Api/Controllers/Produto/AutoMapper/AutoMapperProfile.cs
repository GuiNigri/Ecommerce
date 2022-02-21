using AutoMapper;
using Ecommerce.Api.Controllers.Empresa.Dto;
using Ecommerce.Api.Controllers.Produto.Dto;
using EcommercePrestige.Model.Entity;

namespace Ecommerce.Api.Controllers.Produto.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProdutoCorModel, ObterProdutoPeloCodigoBarrasResponse>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.Referencia, y => y.MapFrom(z => z.ProdutoModel.Referencia))
                .ForMember(x => x.ValorUnitario, y => y.MapFrom(z => z.ProdutoModel.ValorVenda))
                .ForMember(x => x.Cor, y => y.MapFrom(z => z.CodigoInterno));

            CreateMap<EmpresaModel, ObterEmpresaPeloCnpjResponse>()
                .ForMember(x => x.Id, y => y.MapFrom(z => z.Id))
                .ForMember(x => x.RazaoSocial, y => y.MapFrom(z => z.RazaoSocial))
                .ForMember(x => x.Cnpj, y => y.MapFrom(z => z.Cnpj));
        }
    }
}
