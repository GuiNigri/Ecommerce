using System.ComponentModel.DataAnnotations;

namespace EcommercePrestige.Model.Entity
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
    }
}
