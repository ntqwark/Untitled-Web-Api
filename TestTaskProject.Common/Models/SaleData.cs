using System.ComponentModel.DataAnnotations.Schema;

namespace TestTaskProject.Common.Models
{
    public class SaleData
    {
        public int Id { get; set; }
        public Product Product { get; set; }
        public int ProductQuantity { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal ProductIdAmount
        {
            get
            {
                decimal price = Product?.Price ?? 0;

                return price * ProductQuantity;
            }
        }
    }
}