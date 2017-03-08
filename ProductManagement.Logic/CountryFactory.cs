using System.Collections.Generic;
using System.Web.Mvc;

namespace ProductManagement.Logic
{
    public class CountryFactory
    {
        public IEnumerable<SelectListItem> GetCountries()
        {
            List<SelectListItem> countries = new List<SelectListItem>
            {
                new SelectListItem { Text = "Netherlands", Value = "Netherlands" },
                new SelectListItem { Text = "USA", Value = "USA" },
                new SelectListItem { Text = "South Africa", Value = "South Africa" },
                new SelectListItem { Text = "Spain", Value = "Spain" },
                new SelectListItem { Text = "India", Value = "India" }
            };

            return countries;
        }

    }
}
