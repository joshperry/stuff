using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportyGeek.Domain.Abstract;
using SportyGeek.Domain.Concrete;

namespace SportyGeek.WebUI.Controllers
{
    public class ProductsController : Controller
    {
        IProductsRepository _productsRepo;
        public ProductsController()
        {
            _productsRepo = new NHibernateProductsRepository();
        }

        public ActionResult List()
        {
            return View(_productsRepo.Products.ToList());
        }
    }
}
