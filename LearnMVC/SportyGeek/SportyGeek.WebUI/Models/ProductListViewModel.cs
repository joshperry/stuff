using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SportyGeek.Domain.Entities;

namespace SportyGeek.WebUI.Models
{
    public class ProductListViewModel
    {
        public IList<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public string CurrentCategory { get; set; }
    }
}