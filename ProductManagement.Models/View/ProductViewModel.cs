using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ProductManagement.Models.View
{
    public class ProductViewModel
    {
        public int ID { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public int StockLevel { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Priority { get; set; }

        [Required]
        public DateTime ExpireDate { get; set; }

        [Required]
        public bool Retired { get; set; }

        public string Contact { get; set; }

        public IEnumerable<SelectListItem> Countries { get; set; }
    }
}