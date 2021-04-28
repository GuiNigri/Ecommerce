using System;
using System.Collections.Generic;
using System.Text;

namespace EcommercePrestige.Model.Entity
{
    public class AviseMeModel:BaseModel
    {
        public string Email { get; private set; }
        public int ProdutoCorModelId { get; private set; }
        public virtual ProdutoCorModel ProdutoCorModel { get; private set; }

        public AviseMeModel(string email, int produtoCorModelId)
        {
            Email = email;
            ProdutoCorModelId = produtoCorModelId;
        }
    }
}
