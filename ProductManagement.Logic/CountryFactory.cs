using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ProductManagement.Logic
{
    public class CountryFactory
    {
        public IEnumerable<SelectListItem> GetCountries()
        {
            List<SelectListItem> countries = new List<SelectListItem>();
            countries.Add(new SelectListItem { Text = "Netherlands", Value = "Netherlands" });
            countries.Add(new SelectListItem { Text = "USA", Value = "USA" });
            countries.Add(new SelectListItem { Text = "South Africa", Value = "South Africa" });
            countries.Add(new SelectListItem { Text = "Spain", Value = "Spain" });
            countries.Add(new SelectListItem { Text = "India", Value = "India" });

            return countries;
        }

    }
}
