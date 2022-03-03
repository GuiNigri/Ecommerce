using System.Linq;
using AutoMapper;
using EcommercePrestige.Application.ViewModel;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile: Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<EmpresaModel, EmpresaViewModel>();
            CreateMap<EmpresaApiModel, EmpresaViewModel>()
                .ForMember(x => x.RazaoSocial, map => map.MapFrom(y => y.Nome))
                .ForMember(x => x.Cnae, map => map.MapFrom(y => y.Atividade_principal.SingleOrDefault().Code));
            CreateMap<MarcaModel, MarcaViewModel>();
            CreateMap<MaterialModel, MaterialViewModel>();
            CreateMap<ProdutoModel, ProdutoViewModel>()
                .ForMember(x => x.ValorVenda, map => map.MapFrom(y => y.ValorVenda.ToString("N2")))
                .ForMember(x => x.MarcaModel, map => map.MapFrom(y => y.MarcaModel.Nome))
                .ForMember(x => x.MarcaModelId, map => map.MapFrom(y => y.MarcaModel.Id))
                .ForMember(x => x.MaterialModel, map => map.MapFrom(y => y.MaterialModel.Material))
                .ForMember(x => x.MaterialModelId, map => map.MapFrom(y => y.MaterialModel.Id))
                .ForMember(x => x.StatusAtivacao, map => map.MapFrom(y => y.StatusAtivacao));
            CreateMap<UsuarioModel, UsuarioViewModel>();
            CreateMap<SuporteModel, SuporteViewModel>()
                .ForMember(x => x.Documento, map => map.MapFrom(y => y.DocumentoUsuario))
                .ForMember(x => x.Protocolo, map => map.MapFrom(y => y.Id));
            CreateMap<SuporteModel, SuporteInputModel>();
            CreateMap<ProdutoFotoModel, ProdutoFotoViewModel>();
            CreateMap<ProdutoCorModel, ProdutoCorViewModel>()
                .ForMember(x => x.ImgCor, map => map.MapFrom(y => y.CorModel.ImgUrl))
                .ForMember(x => x.CorId, map => map.MapFrom(y => y.CorModelId))
                .ForMember(x => x.DescricaoCor, map => map.MapFrom(y => y.CorModel.Descricao))
                .ForMember(x => x.CodigoInternoCor, map => map.MapFrom(y => y.CodigoInterno))
                .ForMember(x => x.CodigoBarras, map => map.MapFrom(y => y.CodigoBarras));
            CreateMap<CidadesModel, CidadesViewModel>();
            CreateMap<FiltroModel, FiltroProdutoViewModel>();
            CreateMap<KitModel, KitsViewModel>()
                .ForMember(x => x.ValorVenda, map =>map.MapFrom(y=>y.ValorVenda.ToString("N2")));
            CreateMap<TrackingHistoryModel, TrackingHistoryViewModel>();
            CreateMap<PedidoModel, PedidoViewModel>()
                .ForMember(x => x.ValorTotal, map => map.MapFrom(y => y.ValorTotal.ToString("C")));
            CreateMap<PedidoModel, PedidoDetailsViewModel>()
                .ForMember(x => x.ValorTotal, map => map.MapFrom(y => y.ValorTotal.ToString("C")))
                .ForMember(x=>x.SubTotal, map => map.MapFrom(y=>y.Subtotal.ToString("C")))
                .ForMember(x => x.Desconto, map => map.MapFrom(y => y.Desconto.ToString("C")))
                .ForMember(x => x.Frete, map => map.MapFrom(y => y.Frete.ToString("C")))
                .ForMember(x => x.userId, map => map.MapFrom(y => y.UsuarioModel.UserId));
            CreateMap<ProdutoCorModel, ProdutoCorInputModel>()
                .ForMember(x => x.Basic, map => map.MapFrom(y => y.PedidoBasic))
                .ForMember(x => x.Gold, map => map.MapFrom(y => y.PedidoGold))
                .ForMember(x => x.Silver, map => map.MapFrom(y => y.PedidoSilver))
                .ForMember(x => x.ProdutoId, map => map.MapFrom(y => y.ProdutoModelId))
                .ForMember(x => x.Id, map => map.UseDestinationValue())
                .ForMember(x => x.CodigoBarras, map => map.MapFrom(y => y.CodigoBarras));
            CreateMap<ProdutoFotoModel, ProdutoFotoInputModel>()
                .ForMember(x => x.ProdutoId, map => map.MapFrom(y => y.ProdutoModelId))
                .ForMember(x => x.Foto, map => map.MapFrom(y => y.UriBlob))
                .ForMember(x => x.CorId, map => map.MapFrom(y => y.ProdutoCorModel.CorModelId))
                .ForMember(x => x.ImgCor, map => map.MapFrom(y => y.ProdutoCorModel.CorModel.ImgUrl));
            CreateMap<PedidoModel, PedidoAdmViewModel>()
                .ForMember(x => x.ValorTotal, map => map.MapFrom(y => y.ValorTotal.ToString("N2")))
                .ForMember(x => x.Frete, map => map.MapFrom(y => y.Frete.ToString("N2")))
                .ForMember(x => x.Desconto, map => map.MapFrom(y => y.Desconto.ToString("N2")))
                .ForMember(x => x.Usuario, map => map.MapFrom(y => y.UsuarioModel.NomeCompleto))
                .ForMember(x => x.Subtotal, map => map.MapFrom(y => y.Subtotal.ToString("N2")))
                .ForMember(x => x.UsuarioId, map => map.MapFrom(y => y.UsuarioModel.Id))
                .ForMember(x => x.Desconto, map => map.MapFrom(y => y.Desconto.ToString("N2")))
                .ForMember(x => x.UserId, map => map.MapFrom(y => y.UsuarioModel.UserId));
            CreateMap<CorModel, CorViewModel>();
            CreateMap<TextoHomeModel, TextoHomeViewModel>();
            CreateMap<BannersHomeModel, BannersHomeViewModel>();
            CreateMap<AviseMeModel, AviseMeViewModel>()
                .ForMember(x => x.Referencia, map => map.MapFrom(y => y.ProdutoCorModel.ProdutoModel.Referencia))
                .ForMember(x => x.Cor, map => map.MapFrom(y => y.ProdutoCorModel.CodigoInterno));
            CreateMap<CorreioWebServiceModel, CorreioWebServiceViewModel>();
        }
    }
}
