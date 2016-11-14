using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Models.View
{
    public class ProductsViewModel
    {
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}
