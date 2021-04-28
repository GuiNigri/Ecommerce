using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Infrastructure;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;
using Encoder = System.Drawing.Imaging.Encoder;

namespace EcommercePrestige.Services
{
    public class ProdutoFotoServices:BaseServices<ProdutoFotoModel>, IProdutoFotoServices
    {
        private readonly IProdutoFotoRepository _produtoFotoRepository;
        private readonly IBlobInfrastructure _blobInfrastructure;
        private readonly IProdutoCorRepository _produtoCorRepository;
        private readonly IProdutoRepository _produtoRepository;

        public ProdutoFotoServices(IProdutoFotoRepository produtoFotoRepository, IBlobInfrastructure blobInfrastructure,
            IProdutoCorRepository produtoCorRepository, IProdutoRepository produtoRepository) : base(produtoFotoRepository)
        {
            _produtoFotoRepository = produtoFotoRepository;
            _blobInfrastructure = blobInfrastructure;
            _produtoCorRepository = produtoCorRepository;
            _produtoRepository = produtoRepository;
        }

        public async Task<IEnumerable<ProdutoFotoModel>> GetByProdutoAsync(int id)
        {
            return await _produtoFotoRepository.GetByProdutoAsync(id);
        }

        public async Task<ProdutoFotoModel> GetPrincipalByProdutoAsync(int id)
        {
            return await _produtoFotoRepository.GetPrincipalByProdutoAsync(id);
        }

        public async Task<ProdutoFotoModel> GetFotosToKitAsync(int id)
        {
            return await _produtoFotoRepository.GetFotosToKitAsync(id);
        }

        public async Task<ProdutoFotoModel> GetFotosbyCorIndexAsync(int id)
        {
            return await _produtoFotoRepository.GetFotosbyCorIndexAsync(id);
        }

        public async Task<IEnumerable<ProdutoFotoModel>> GetFotosbyCorAsync(int id)
        {

            return await _produtoFotoRepository.GetFotosbyCorAsync(id);
        }

        public async Task<(bool,string)> AddFotoList(Stream stream, int idCor,ProdutoFotoModel produtoFotoModel, string statusAtivacao)
        {
            var produtoCorModel = await _produtoCorRepository.GetCorByProdutoAsync(idCor,
                produtoFotoModel.ProdutoModelId, statusAtivacao);

            var streamResized = await ResizeImage(stream);

            var uriBlob = await _blobInfrastructure.CreateBlobAsync(streamResized, "produtosblob");

            produtoFotoModel.SetUriBlob(uriBlob);
            produtoFotoModel.SetProdutoCorModelId(produtoCorModel.Id);

            if (produtoFotoModel.Principal)
            {
                var verificacao = await _produtoFotoRepository.CheckFotoAndPrincipal(produtoFotoModel.ProdutoModelId);

                if (verificacao)
                {
                    return (false, "O produto já tem uma foto principal cadatrada");
                }
            }
            
            await _produtoFotoRepository.CreateAsync(produtoFotoModel);

            return (true, null);
        }

        public async Task RemoveFotoList(int id)
        {
            var produtoFotoModel = await _produtoFotoRepository.GetByIdAsync(id);

            await _blobInfrastructure.DeleteBlobAsync(produtoFotoModel.UriBlob, "produtosblob");

            await _produtoFotoRepository.DeleteAsync(id);
        }

        public Task<IEnumerable<ProdutoFotoModel>> RetornarListaFotoInput(int idProd)
        {
            return _produtoFotoRepository.RetornarListaFotoInput(idProd);
        }

        public async Task CreateAsync(int idProduto, string statusAtivacao)
        {
            var listaCoresDoProduto = await _produtoCorRepository.GetByProdutoAsync(idProduto,statusAtivacao);

            foreach (var cores in listaCoresDoProduto)
            {
                cores.SetStatusAtivacao("AT");
                await _produtoCorRepository.UpdateAsync(cores);
            }

            var listaFotoDoProduto = await _produtoFotoRepository.GetByProdutoAsync(idProduto);

            foreach (var fotos in listaFotoDoProduto)
            {
                fotos.SetStatusAtivacao("AT");
                await _produtoFotoRepository.UpdateAsync(fotos);
            }

            var produtoModel = await _produtoRepository.GetByIdAsync(idProduto);

            produtoModel.SetStatusAtivacao("AT");

            await _produtoRepository.UpdateAsync(produtoModel);
        }

        public async Task<bool> CheckFotoAndPrincipal(int idProd)
        {
            return await _produtoFotoRepository.CheckFotoAndPrincipal(idProd);
        }

        public async Task UpdateAsync(int id)
        {
            var produtoModel = await _produtoFotoRepository.GetByIdAsync(id);

            produtoModel.SetStatusAtivacao(produtoModel.StatusAtivacao == "AT" ? "IN" : "AT");

            await _produtoFotoRepository.UpdateAsync(produtoModel);
        }

        private async Task<Stream> ResizeImage(Stream imageStream)
        {

            var imageFromStream = Image.FromStream(imageStream);

            var destRect = new Rectangle(0, 0, imageFromStream.Width, imageFromStream.Height);
            var destImage = new Bitmap(imageFromStream.Width, imageFromStream.Height, PixelFormat.Format24bppRgb);

            destImage.SetResolution(72, 72);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);

                    graphics.DrawImage(imageFromStream, destRect, 0, 0, imageFromStream.Width, imageFromStream.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            var param = new EncoderParameters();

            param.Param[0] = new EncoderParameter(Encoder.Quality, 80L);

            var codec = ImageCodecInfo.GetImageDecoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);

            var ms = new MemoryStream();

            destImage.Save(ms, codec, param);

            ms.Position = 0;

            return ms;

        }
    }
}
