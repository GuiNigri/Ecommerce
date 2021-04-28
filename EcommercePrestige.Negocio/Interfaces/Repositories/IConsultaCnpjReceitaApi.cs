using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Repositories
{
    public interface IConsultaCnpjReceitaApi
    {
        (bool,string) GetCaptcha();
        (bool,string) ConsultaDados(string aCnpj, string aCaptcha);
        EmpresaModel MontarDadosEmpresa(string cnpj, string resp);
        string ObterDados(string aCnpj, string aCaptcha);
        (bool,string) ValidaCampos(string aCnpj, string aCaptcha);
        string RetornarMensagem();
        byte[] RetornarImgCaptcha();
        EmpresaModel RetornarEmpresa();

    }
}
