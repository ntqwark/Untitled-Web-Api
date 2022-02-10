using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskProject.Common.Models
{
    public class SalesPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ProvidedProduct> ProvidedProducts { get; set; } = new List<ProvidedProduct>();
    }
}
