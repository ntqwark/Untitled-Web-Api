using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TestTaskProject.Common.Models
{
    public class MakeSale
    {
        public int? BuyerId { get; set; }
        [Required]
        public int SalePointId { get; set; }
        [Required]
        public List<ProductToBuy> ProductsToBuy { get; set; } = new List<ProductToBuy>();
    }
}
