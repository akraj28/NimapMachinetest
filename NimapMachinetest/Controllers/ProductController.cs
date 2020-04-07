using NimapMachinetest.Models;
using NimapMachinetest.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace NimapMachinetest.Controllers
{
    public class ProductController : Controller
    {
        DataDBContext dBContext;
        private const int pageSize = 20;

        public ProductController()
        {
            dBContext = new DataDBContext();
        }

        // GET: Product
        public ActionResult Index(int? id)
        {
            int Page = 1;
            List<ProductViewModel> model = null;
            if(id != null)
            {
                Page = Convert.ToInt32(id);
            }
            model = GetProducts(Page);
            return View(model);
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product model)
        {          

            dBContext.Products.Add(model);
            dBContext.SaveChanges();
            return Json("Success", JsonRequestBehavior.AllowGet);
        }        

        private List<ProductViewModel> GetProducts(int currentPage)
        {

            int maxRows = 2;
            using (dBContext = new DataDBContext())
            {
                List<ProductViewModel> model = new List<ProductViewModel>();
                model = (from p in dBContext.Products
                            join c in dBContext.Categories on p.CategoryId equals c.CategoryId

                            select new ProductViewModel
                            {
                                ProductId = p.ProductId,
                                ProductName = p.ProductName,
                                CategoryId = c.CategoryId,
                                CategoryName = c.CategoryName

                            })
                                     .OrderBy(product => product.ProductId)
                                     .Skip((currentPage - 1) * maxRows)
                                     .Take(maxRows).ToList();


                double pageCount = (double)((decimal)dBContext.Products.Count() / Convert.ToDecimal(maxRows));
                ViewBag.PageCount = (int)Math.Ceiling(pageCount);

                ViewBag.CurrentPageIndex = currentPage;

                return model;
            }
        }

        public ActionResult Edit(int id)
        {
            Product model = (from Product in dBContext.Products
                              where id.Equals(Product.ProductId)
                              select Product).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            try
            {
                dBContext.Entry(product).State = EntityState.Modified;

                dBContext.SaveChanges();
                return Json("Success", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            Product model = (from product in dBContext.Products
                              where id.Equals(product.ProductId)
                              select product).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            Product model = (from product in dBContext.Products
                             where id.Equals(product.ProductId)
                             select product).FirstOrDefault();

            dBContext.Products.Remove(model);
            dBContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}