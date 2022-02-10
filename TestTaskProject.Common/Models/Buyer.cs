using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskProject.Common.Models
{
    public class Buyer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // public IEnumerable<int> SalesIds { get; set; }
        public List<Sale> Sales { get; set; } = new List<Sale>();
    }
}
