namespace EcommercePrestige.Model.Entity
{
    public class ProdutoModel : BaseModel
    {
        public int MarcaModelId { get; private set; }
        public virtual MarcaModel MarcaModel { get; private set; }
        public int MaterialModelId { get; private set; }
        public virtual MaterialModel MaterialModel { get; private set; }
        public string Referencia { get; private set; }
        public string Tamanho { get; private set; }
        public string Descricao { get; private set; }
        public double ValorVenda { get; private set; }
        public string StatusProduto { get; private set; }
        public string Genero { get; private set; }
        public bool BestSeller { get; private set; }
        public bool Liquidacao { get; private set; }
        public bool Vitrine { get; private set; }
        public string StatusAtivacao { get; private set; }

        public ProdutoModel() { }
        public ProdutoModel(int id, int marcaModelId, MarcaModel marcaModel, int materialModelId, MaterialModel materialModel, string referencia, string tamanho, string descricao, double valorVenda, string statusProduto, string genero)
        {
            Id = id;
            MarcaModelId = marcaModelId;
            MarcaModel = marcaModel;
            MaterialModelId = materialModelId;
            MaterialModel = materialModel;
            Referencia = referencia;
            Tamanho = tamanho;
            Descricao = descricao;
            ValorVenda = valorVenda;
            StatusProduto = statusProduto;
            Genero = genero;
        }

        public void SetStatusAtivacao(string statusAtivacao)
        {
            StatusAtivacao = statusAtivacao;
        }

        public void SetReferencia(string referencia)
        {
            Referencia = referencia;
        }


    }

    public class ProdutoFotoModel : BaseModel
    {
        public string UriBlob { get; private set; }
        public bool Principal { get; private set; }
        public int ProdutoModelId { get; private set; }
        public virtual ProdutoModel ProdutoModel { get; private set; }
        public int ProdutoCorModelId { get; private set; }
        public virtual ProdutoCorModel ProdutoCorModel { get; private set; }
        public string StatusAtivacao { get; private set; }

        public ProdutoFotoModel() { }

        public ProdutoFotoModel(string uriBlob)
        {
            UriBlob = uriBlob;
        }

        public void SetUriBlob(string uriBlob)
        {
            UriBlob = uriBlob;
        }

        public void SetStatusAtivacao(string statusAtivacao)
        {
            StatusAtivacao = statusAtivacao;
        }

        public void SetProdutoCorModelId(int produtoCorModelId)
        {
            ProdutoCorModelId = produtoCorModelId;
        }
    }

    public class ProdutoCorModel : BaseModel
    {
        public int CorModelId { get; private set; }
        public virtual CorModel CorModel { get; private set; }
        public string CodigoInterno { get; private set; }
        public int Estoque { get; private set; }
        public int ProdutoModelId { get; private set; }
        public virtual ProdutoModel ProdutoModel { get; private set; }
        public bool PedidoGold { get; private set; }
        public bool PedidoSilver { get; private set; }
        public bool PedidoBasic { get; private set; }
        public string StatusAtivacao { get; private set; }
        public string CodigoBarras { get; private set; }

        public void SetEstoque(int estoque)
        {
            Estoque = estoque;
        }

        public void AtualizarCodigoBarras(string codigoBarras)
        {
            CodigoBarras = codigoBarras;
        }

        public void SetStatusAtivacao(string statusAtivacao)
        {
            StatusAtivacao = statusAtivacao;
        }

        public void SetPedidoGold(bool pedidoGold)
        {
            PedidoGold = pedidoGold;
        }

        public void SetPedidoSilver(bool pedidoSilver)
        {
            PedidoSilver = pedidoSilver;
        }

        public void SetPedidoBasic(bool pedidoBasic)
        {
            PedidoBasic = pedidoBasic;
        }
    }

    public class CorModel : BaseModel
    {
        public string ImgUrl { get; private set; }
        public string Descricao { get; private set; }
    }

}


