namespace EcommercePrestige.Model.Entity
{
    public class FiltroModel:BaseModel
    {
        public int CorOption { get; private set; }
        public int MarcaOption { get; private set; }
        public string GeneroOption { get; private set; }
        public string OrderType { get; private set; }
        public string Category { get; private set; }
        public int MaterialOption { get; private set; }
        public string Termo { get; private set; }

        public void SetMarcaOption(int marcaOption)
        {
            MarcaOption = marcaOption;
        }

        public void SetMaterialOption(int materialOption)
        {
            MarcaOption = materialOption;
        }
    }
}
