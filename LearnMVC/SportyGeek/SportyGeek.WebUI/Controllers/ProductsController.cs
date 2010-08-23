using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportyGeek.Domain.Abstract;
using SportyGeek.Domain.Concrete;
using SportyGeek.Domain.Entities;
using SportyGeek.WebUI.Models;

namespace SportyGeek.WebUI.Controllers
{
    public class ProductsController : Controller
    {
        IEntityRepository<Product> _productsRepo;
        public ProductsController(IEntityRepository<Product> repo)
        {
            _productsRepo = repo;
            PageSize = 3;
        }

        public ViewResult List(int page = 1, string category = null)
        {
            var products = category == null
                ? _productsRepo.Query
                : _productsRepo.Query.Where(p => p.Category == category);

            var model = new ProductListViewModel
            {
                Products = products
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList(),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = products.Count()
                },
                CurrentCategory = category
            };

            return View(model);
        }

        public ViewResult HelloWorld()
        {
            return View();
        }

        public int PageSize { get; set; }
    }
}
