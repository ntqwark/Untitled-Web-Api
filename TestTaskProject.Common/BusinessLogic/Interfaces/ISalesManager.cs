using TestTaskProject.Common.Models;

namespace TestTaskProject.Common.BusinessLogic
{
    public interface ISalesManager
    {
        public MakeSaleResult CreateSale(MakeSale model, SalesPoint salePoint, Buyer buyer);
    }
}
