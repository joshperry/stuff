﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportyGeek.Domain.Entities;
using SportyGeek.WebUI.Controllers;
using Moq;
using SportyGeek.Domain.Abstract;
using System.Collections;
using SportyGeek.WebUI.Models;

namespace SportyGeek.Tests
{
    [TestClass]
    public class CatalogTests
    {
        [TestMethod]
        public void Can_View_A_Single_Page_Of_Products()
        {
            var products = new Product[] {
                new Product { Name="P1" },
                new Product { Name="P2" },
                new Product { Name="P3" },
                new Product { Name="P4" },
                new Product { Name="P5" },
                new Product { Name="P6" },
            };

            var secondPage = new Product[3];
            Array.Copy(products, 3, secondPage, 0, 3);

            var repo = new Mock<IEntityRepository<Product>>();
            repo.SetupGet(r => r.Query).Returns(products.AsQueryable());

            var controller = new ProductsController(repo.Object);
            controller.PageSize = 3;

            var result = controller.List(2);
            EnumerableAssert.AreEqual(secondPage, ((ProductListViewModel)result.ViewData.Model).Products);
        }

        [TestMethod]
        public void Product_paging_defaults_to_1_pagesize_to_3()
        {
            var products = new Product[] {
                new Product { Name="P1" },
                new Product { Name="P2" },
                new Product { Name="P3" },
                new Product { Name="P4" },
                new Product { Name="P5" },
                new Product { Name="P6" },
            };

            var repo = new Mock<IEntityRepository<Product>>();
            repo.SetupGet(r => r.Query).Returns(products.AsQueryable());

            var controller = new ProductsController(repo.Object);

            var result = controller.List();
            Assert.AreEqual(
                new PagingInfo { CurrentPage = 1, ItemsPerPage = 3, TotalItems = products.Length },
                ((ProductListViewModel)result.ViewData.Model).PagingInfo
            );
        }

        [TestMethod]
        public void Product_list_calculates_paging_info()
        {
            var products = new Product[] {
                new Product { Name="P1" },
                new Product { Name="P2" },
                new Product { Name="P3" },
                new Product { Name="P4" },
                new Product { Name="P5" },
                new Product { Name="P6" },
            };

            var repo = new Mock<IEntityRepository<Product>>();
            repo.SetupGet(r => r.Query).Returns(products.AsQueryable());

            var controller = new ProductsController(repo.Object);

            var result = controller.List(2);
            var viewModel = (ProductListViewModel)result.ViewData.Model;
            Assert.AreEqual(viewModel.PagingInfo, new PagingInfo { TotalItems = 6, ItemsPerPage = 3, CurrentPage = 2 });
        }

        [TestMethod]
        public void Default_list_products_from_all_Categories()
        {
            var products = new Product[] {
                new Product { Name="P1", Category= "C1" },
                new Product { Name="P2", Category= "C2" },
            };

            var repo = new Mock<IEntityRepository<Product>>();
            repo.SetupGet(r => r.Query).Returns(products.AsQueryable());

            var controller = new ProductsController(repo.Object);
            controller.PageSize = 2;

            var result = controller.List(1);
            EnumerableAssert.AreEqual(products, ((ProductListViewModel)result.ViewData.Model).Products);
        }

        [TestMethod]
        public void Can_filter_products_by_category()
        {
            var products = new Product[] {
                new Product { Name="P1", Category= "C1" },
                new Product { Name="P2", Category= "C2" },
            };

            var repo = new Mock<IEntityRepository<Product>>();
            repo.SetupGet(r => r.Query).Returns(products.AsQueryable());

            var controller = new ProductsController(repo.Object);
            controller.PageSize = 2;

            var result = controller.List(1, "C1");
            var viewModel = result.ViewData.Model as ProductListViewModel;
            EnumerableAssert.AreEqual(products.Where(p => p.Category == "C1"), viewModel.Products);
        }

        [TestMethod]
        public void Category_filter_sets_viewmodel_category()
        {
            var products = new Product[] {
                new Product { Name="P1", Category= "C1" }
            };

            var repo = new Mock<IEntityRepository<Product>>();
            repo.SetupGet(r => r.Query).Returns(products.AsQueryable());
            var controller = new ProductsController(repo.Object);

            var result = controller.List(1, "C1");
            Assert.AreEqual("C1", ((ProductListViewModel)result.ViewData.Model).CurrentCategory);
        }
    }
}
