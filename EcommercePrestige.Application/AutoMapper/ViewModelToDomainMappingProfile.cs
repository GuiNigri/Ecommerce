using AutoMapper;
using EcommercePrestige.Application.ViewModel;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile:Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<EmpresaViewModel, EmpresaModel>();
            CreateMap<MarcaViewModel, MarcaModel>();
            CreateMap<MaterialViewModel, MaterialModel>();
            CreateMap<ProdutoViewModel, ProdutoModel>()
                .ForMember(x => x.ValorVenda, map => map.MapFrom(y => double.Parse(y.ValorVenda)));
            CreateMap<UsuarioViewModel, UsuarioModel>();
            CreateMap<SuporteViewModel, SuporteModel>()
                .ForMember(x => x.DocumentoUsuario, map => map.MapFrom(y=>y.Documento));
            CreateMap<SuporteInputModel, SuporteModel>()
                .ForMember(x => x.Cnpj, map => map.Ignore())
                .ForMember(x => x.Cpf, map => map.Ignore())
                .ForMember(x => x.DocumentoUsuario, map => map.Ignore())
                .ForMember(x => x.Assunto, map => map.MapFrom(y => y.Assuntos));
            CreateMap<ProdutoFotoViewModel, ProdutoFotoModel>();
            CreateMap<ProdutoCorViewModel, ProdutoCorModel>();
            CreateMap<CidadesViewModel, CidadesModel>();
            CreateMap<FiltroProdutoViewModel, FiltroModel>()
                .ForMember(x => x.MaterialOption, x => x.Ignore())
                .ForMember(x => x.MarcaOption, x => x.Ignore());
            CreateMap<KitsViewModel, KitModel>()
                .ForMember(x => x.ValorVenda, map => map.MapFrom(y => double.Parse(y.ValorVenda)));
            CreateMap<ProdutoCreateEtapaBasicaModel, ProdutoModel>();
            CreateMap<ProdutoCorInputModel, ProdutoCorModel>()
                .ForMember(x => x.PedidoBasic, map => map.MapFrom(y => y.Basic))
                .ForMember(x => x.PedidoGold, map => map.MapFrom(y => y.Gold))
                .ForMember(x => x.PedidoSilver, map => map.MapFrom(y => y.Silver))
                .ForMember(x => x.ProdutoModelId, map => map.MapFrom(y => y.ProdutoId))
                .ForMember(x => x.CorModelId, map => map.MapFrom(y => y.CorId));
            CreateMap<ProdutoFotoInputModel, ProdutoFotoModel>()
                .ForMember(x=>x.ProdutoModelId, map=>map.MapFrom(y=>y.ProdutoId))
                .ForMember(x => x.UriBlob, map => map.MapFrom(y => y.Foto));
            CreateMap<PedidoAdmViewModel, PedidoModel>()
                .ForMember(x => x.ValorTotal, map => map.MapFrom(y => double.Parse(y.ValorTotal)))
                .ForMember(x => x.UsuarioModelId, map => map.MapFrom(y => y.UsuarioId))
                .ForMember(x => x.Subtotal, map => map.MapFrom(y => double.Parse(y.Subtotal)))
                .ForMember(x => x.Desconto, map => map.MapFrom(y => double.Parse(y.Desconto)))
                .ForMember(x => x.Frete, map => map.MapFrom(y => double.Parse(y.Frete)));
            CreateMap<CorViewModel, CorModel>();
            CreateMap<TextoHomeViewModel, TextoHomeModel>();
            CreateMap<BannersHomeViewModel, BannersHomeModel>();
        }
    }
}
