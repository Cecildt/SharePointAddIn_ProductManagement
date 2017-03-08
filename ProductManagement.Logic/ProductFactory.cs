using ProductManagement.Models.View;
using System.Collections.Generic;
using System.Linq;
using ProductManagement.DAL.Interfaces;
using ProductManagement.DAL;
using ProductManagement.Models.Data;
using ProductManagement.Logic.Helpers;

namespace ProductManagement.Logic
{
    public class ProductFactory
    {
        private IProductRepository _repo = new ProductMemoryRepository(); // TODO: Change to use Dependency Injection
        private CountryFactory _countryFactory = new CountryFactory();

        public ProductFactory()
        {
        }

        public IEnumerable<ProductViewModel> GetNonRetiredProducts()
        {
            List<ProductDataModel> products = _repo.All().Where(p => p.Retired == false).ToList();

            return MapModels.ToProductViewModelList(products, _countryFactory.GetCountries());
        }

        public IEnumerable<ProductViewModel> GetRetiredProducts()
        {
            List<ProductDataModel> products = _repo.All().Where(p => p.Retired == true).ToList();

            return MapModels.ToProductViewModelList(products, _countryFactory.GetCountries());
        }

        public IEnumerable<ProductViewModel> GetProductByCountry(string countryName)
        {
            List<ProductDataModel> products = _repo.All().Where(p => p.Country == countryName).ToList();

            return MapModels.ToProductViewModelList(products, _countryFactory.GetCountries());
        }

        public IEnumerable<ProductViewModel> GetHighProducts()
        {
            List<ProductDataModel> products = _repo.All().Where(p => p.Priority == "High").ToList();

            return MapModels.ToProductViewModelList(products, _countryFactory.GetCountries());
        }

        public ProductViewModel GetProductByID(int id)
        {
            ProductDataModel dataModel = _repo.Get(id);

            return MapModels.ToProductViewModel(dataModel, _countryFactory.GetCountries());
        }

        public bool AddProduct(ProductViewModel model)
        {
            ProductDataModel dataModel = MapModels.ToProductDataModel(model);

            return _repo.Add(dataModel);
        }
        
        public bool UpdateProduct(ProductViewModel model)
        {
            ProductDataModel dataModel = MapModels.ToProductDataModel(model);

            return _repo.Update(dataModel);
        }

        public bool DeleteProduct(ProductViewModel model)
        {
            return _repo.Delete(model.ID);
        }

    }
}
