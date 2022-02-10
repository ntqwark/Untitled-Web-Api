using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskProject.Common.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }
        // public int SalesPointId { get; set; }
        public SalesPoint SalesPoint { get; set; }
        // public int BuyerId { get; set; }
        public Buyer Buyer { get; set; }
        public List<SaleData> SaleData { get; set; } = new List<SaleData>();

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal TotalAmount
        {
            get
            {
                if (SaleData == null)
                {
                    return 0;
                }

                decimal totalAmount = 0;

                foreach (SaleData data in SaleData)
                {
                    totalAmount += data.ProductIdAmount;
                }

                return totalAmount;
            }
        }
    }
}
