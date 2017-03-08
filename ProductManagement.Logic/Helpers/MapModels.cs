using ProductManagement.Models.Data;
using ProductManagement.Models.View;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ProductManagement.Logic.Helpers
{
    static class MapModels
    {
        public static IEnumerable<ProductViewModel> ToProductViewModelList(IEnumerable<ProductDataModel> dataModels, IEnumerable<SelectListItem> countries)
        {
            List<ProductViewModel> viewModels = new List<ProductViewModel>();

            foreach (ProductDataModel item in dataModels)
            {
                viewModels.Add(new ProductViewModel()
                {
                    ID = item.ID,
                    Title = item.Title,
                    Priority = item.Priority,
                    Retired = item.Retired,
                    Country = item.Country,
                    ExpireDate = item.ExpireDate,
                    StockLevel = item.StockLevel,
                    Countries = countries
                });
            }

            return viewModels;
        }

        public static IEnumerable<ProductDataModel> ToProductDataModelList(IEnumerable<ProductViewModel> viewModels)
        {
            List<ProductDataModel> dataModels = new List<ProductDataModel>();

            foreach (ProductViewModel item in viewModels)
            {
                dataModels.Add(new ProductDataModel()
                {
                    ID = item.ID,
                    Title = item.Title,
                    Priority = item.Priority,
                    Retired = item.Retired,
                    Country = item.Country,
                    ExpireDate = item.ExpireDate,
                    StockLevel = item.StockLevel
                });
            }

            return dataModels;
        }
        
        public static ProductViewModel ToProductViewModel(ProductDataModel dataModel, IEnumerable<SelectListItem> countries)
        {
            return new ProductViewModel()
            {
                ID = dataModel.ID,
                Title = dataModel.Title,
                Priority = dataModel.Priority,
                Retired = dataModel.Retired,
                Country = dataModel.Country,
                ExpireDate = dataModel.ExpireDate,
                StockLevel = dataModel.StockLevel,
                Countries = countries
            };
        }
        
        public static ProductDataModel ToProductDataModel(ProductViewModel viewModel)
        {
            return new ProductDataModel()
            {
                ID = viewModel.ID,
                Title = viewModel.Title,
                Priority = viewModel.Priority,
                Retired = viewModel.Retired,
                Country = viewModel.Country,
                ExpireDate = viewModel.ExpireDate,
                StockLevel = viewModel.StockLevel
            };
        }        
    }
}
