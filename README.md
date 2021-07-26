# Ecommerce

Modelo de um ecommerce completo construido em C#, atualmente em funcionamento no site: https://prestigedobrasil.com.br, projeto aberto a sugestões e melhorias!

## 🚀 Começando

Essas instruções permitirão que você obtenha uma cópia do projeto em operação na sua máquina local para fins de desenvolvimento e teste.

### 📋 Pré-requisitos

O software está pronto para funcionamento, faltando apenas o aquivo appsettings.json, aonde contem os arquivos de configuração, segue modelo:

```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "EmailSettings": { //configurações de acordo com o seu provedor de email.
    "PrimaryDomain": "",
    "PrimaryPort": "",
    "UsernameEmail": "",
    "UsernamePassword": "",
    "FromEmail": "fromEmail"
  },
  "PagarMeSettings": { //chaves da API do PagarMe
    "DefaultApiKey": "",
    "DefaultEncryptionKey": ""
  },
  "CieloSettings": { //chaves da API da Cielo
    "MerchantId": ""
  },
  "AllowedHosts": "*",
  "ConnectionStrings": { //configurações das conexões com o banco de dados MSSQL e BLOB Azure - (Entity Framework)
    "StorageAccount": "",
    "IdentityContextConnection": "",
    "EcommerceContext": ""
  }
}
```

É nescessario executar os comandos a baixo, para implementar o banco de dados de teste, após adicionar as connections strings nas configurações! (lembrando sempre de apontar para o projeto padrão "Data")
```
add-migration migration_name
```
e depois,
```
update-database –verbose
```
## 🛠️ Tecnologias Utilizadas

* ASP .Net 5 - Framework WEB
* C# - Linguagem
* Entity Framework
* Identity Framework
* MSSQL
* Azure Blob

## ✒️ Autores

* **Guilherme Tofic Nigri** - *Desenvolvimento* - [Guilherme Tofic Nigri](https://github.com/GuiNigri/)
