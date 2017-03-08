using ProductManagement.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using ProductManagement.Models.Data;

namespace ProductManagement.DAL
{
    public class ProductMemoryRepository : IProductRepository
    {
        private static List<ProductDataModel> _products = new List<ProductDataModel>()
        {
            new ProductDataModel()
            {
                ID = 1,
                Title = "Product AA",
                Priority = "High",
                ExpireDate = new DateTime(2017, 4, 25),
                Country = "Netherlands",
                Retired = false,
                StockLevel = 200
            },
            new ProductDataModel()
            {
                ID = 2,
                Title = "Product BB",
                Priority = "Medium",
                ExpireDate = new DateTime(2017, 7, 31),
                Country = "Spain",
                Retired = true,
                StockLevel = 599
            },
            new ProductDataModel()
            {
                ID = 3,
                Title = "Product CC",
                Priority = "Low",
                ExpireDate = new DateTime(2017, 6, 30),
                Country = "Spain",
                Retired = false,
                StockLevel = 15788
            },
            new ProductDataModel()
            {
                ID = 1,
                Title = "Product DD",
                Priority = "High",
                ExpireDate = new DateTime(2017, 10, 12),
                Country = "India",
                Retired = false,
                StockLevel = 2000
            }
        };

        public ProductMemoryRepository()
        {

        }


        public bool Add(ProductDataModel model)
        {
            int lastID = _products.Select(x => x.ID).Max();

            model.ID = lastID + 1;

            _products.Add(model);

            return true;
        }

        public IEnumerable<ProductDataModel> All()
        {
            return _products;
        }

        public bool Delete(int id)
        {
            ProductDataModel product = _products.Where(p => p.ID == id).FirstOrDefault();

            if (product == null)
            {
                return false;
            }

            _products.Remove(product);

            return true;
        }

        public ProductDataModel Get(int id)
        {
            return _products.Where(p => p.ID == id).FirstOrDefault();
        }

        public bool Update(ProductDataModel model)
        {
            ProductDataModel product = _products.Where(p => p.ID == model.ID).FirstOrDefault();

            if (product == null)
            {
                return false;
            }

            product.Title = model.Title;
            product.Priority = model.Priority;
            product.Retired = model.Retired;
            product.StockLevel = model.StockLevel;
            product.Country = model.Country;
            product.ExpireDate = model.ExpireDate;

            return true;
        }
    }
}
