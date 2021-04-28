using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;
using Newtonsoft.Json;

namespace EcommercePrestige.Services
{
    public class EmpresaServices:BaseServices<EmpresaModel>, IEmpresaServices
    {
        private readonly IConsultaCnpjAwsApi _consultaCnpjAwsApi;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public EmpresaServices(IConsultaCnpjAwsApi consultaCnpjAwsApi, IEmpresaRepository empresaRepository, IUsuarioRepository usuarioRepository)
            :base(empresaRepository)
        {
            _consultaCnpjAwsApi = consultaCnpjAwsApi;
            _empresaRepository = empresaRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task CreateAsync(EmpresaModel empresaModel, UsuarioModel usuarioModel)
        {
            await _empresaRepository.CreateAsync(empresaModel);
            await _usuarioRepository.CreateAsync(usuarioModel);
        }

        public async Task<EmpresaApiModel> GetDadosEmpresaAsync(string cnpj)
        {
            return await _consultaCnpjAwsApi.consultarCNPJ(cnpj);
        }


        public async Task<IEnumerable<CidadesModel>> GetCidadesEmpresasAsync()
        {
            return await _empresaRepository.GetCidadesEmpresasAsync();
        }

        public async Task<IEnumerable<object>> GetBairrosByCidadesAsync(CidadesModel cidadesModel)
        {
            return await _empresaRepository.GetBairrosByCidadesAsync(cidadesModel);
        }

        public async Task<IEnumerable<EmpresaModel>> GetListaDeEmpresasByCidadesEBairro(string cidade, string bairro)
        {
            if (cidade.ToLower() == "brasilia" && bairro.ToLower() == "asa")
            {
                bairro = "ASA NORTE";
            }
            else if (cidade.ToLower() == "rio de janeiro" && bairro.ToLower() == "barra")
            {
                bairro = "BARRA DA TIJUCA";
            }
            return await _empresaRepository.GetListaDeEmpresasByCidadesEBairro(cidade, bairro);
        }

        public async Task<EmpresaModel> GetEmpresaByCnpj(string cnpj)
        {
            return await _empresaRepository.GetEmpresaByCnpj(cnpj);
        }

        public async Task<EmpresaModel> GetEmpresaByUserId(string userId)
        {
            return await _empresaRepository.GetEmpresaByUserId(userId);
        }

        public async Task<IEnumerable<EmpresaModel>> FilterAsync(string termo)
        {
            return await _empresaRepository.FilterAsync(termo);
        }
    }
}
