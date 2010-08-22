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

        public ViewResult List(int page = 1)
        {
            var model = new ProductListViewModel
            {
                Products = _productsRepo.Query
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList(),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _productsRepo.Query.Count()
                }
            };

            return View(model);
        }

        public int PageSize { get; set; }
    }
}
