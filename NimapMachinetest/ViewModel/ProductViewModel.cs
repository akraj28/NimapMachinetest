using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NimapMachinetest.ViewModel
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
       // public List<ProductViewModel> ProdList { get; set; }

        public int CurrentPageIndex { get; set; }

        public int PageCount { get; set; }
    }
}