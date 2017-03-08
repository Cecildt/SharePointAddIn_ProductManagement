using ProductManagement.Models.Data;
using System.Collections.Generic;

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
