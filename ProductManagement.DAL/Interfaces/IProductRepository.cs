using ProductManagement.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.DAL.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<ProductDataModel> All();

        ProductDataModel Get(int id);

        bool Add(ProductDataModel model);

        bool Update(ProductDataModel model);

        bool Delete(int id);

    }
}
