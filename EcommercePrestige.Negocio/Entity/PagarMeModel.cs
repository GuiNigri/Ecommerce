using System;
using System.Collections.Generic;
using System.Text;

namespace EcommercePrestige.Model.Entity
{

    public class PagarMeSettingsModel
    {
        public string DefaultApiKey { get; set; }
        public string DefaultEncryptionKey { get; set; }

    }

    public class PagarMeModel
    {
        public int Amount { get; private set; }
        public string Card_hash { get; private set; }
        public string Installments { get; private set; }
        public string Payment_method { get; private set; }
        public PagarMeCustomerModel Customer { get; private set; }

        public void SetInstallments(string installments)
        {
            Installments = installments;
        }
    }

    public class PagarMeCustomerModel
    {
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Document_number { get; private set; }
        public PagarMeAddressModel Address { get; private set; }
        public PagarMePhoneModel Phone { get; private set; }
    }

    public class PagarMeAddressModel
    {
        public string City { get; private set; }
        public string Complementary { get; private set; }
        public string Neighborhood { get; private set; }
        public string State { get; private set; }
        public string Street { get; private set; }
        public string Street_number { get; private set; }
        public string Zipcode { get; private set; }
    }

    public class PagarMePhoneModel
    {
        public string Ddd { get; private set; }
        public string Number { get; private set; }
    }

    public class PagarMeReturnModel
    {
        public string TransactionStatus { get; private set; }
        public string Tid { get; private set; }
        public string AuthorizationCode { get; private set; }

        public PagarMeReturnModel(string transactionStatus, string tid, string authorizationCode)
        {
            TransactionStatus = transactionStatus;
            Tid = tid;
            AuthorizationCode = authorizationCode;
        }
    }
}
