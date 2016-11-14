using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Models.Data
{
    public class ProductDataModel
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public int StockLevel { get; set; }

        public string Country { get; set; }

        public string Priority { get; set; }

        public DateTime ExpireDate { get; set; }

        public bool Retired { get; set; }
    }
}
