namespace TestTaskProject.Common.Models
{
    public class ProvidedProduct
    {
        public int Id { get; set; }
        //public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ProductQuantity { get; set; }
    }
}