using NimapMachinetest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace NimapMachinetest.Controllers
{
    public class CategoryController : Controller
    {
        DataDBContext dBContext;

        public CategoryController()
        {
            dBContext = new DataDBContext();
        }
        // GET: Category

        public ActionResult List()
        {
            List<Category> list = dBContext.Categories.ToList();

            return View(list);
        } 

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category model)
        {
            int categoryId = (from Category in dBContext.Categories
                              where model.CategoryName.Equals(Category.CategoryName)
                              select Category.CategoryId).FirstOrDefault();

            if(categoryId!=0)
                return Json("Failure", JsonRequestBehavior.AllowGet);


            dBContext.Categories.Add(model);
            dBContext.SaveChanges();
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(int id)
        {
            Category model = (from category in dBContext.Categories
                              where id.Equals(category.CategoryId)
                              select category).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(Category model)
        {
            try
            {
                int categoryId = (from Category in dBContext.Categories
                                  where model.CategoryName.Equals(Category.CategoryName)
                                  select Category.CategoryId).FirstOrDefault();

                if (categoryId != 0)
                    return Json("Failure", JsonRequestBehavior.AllowGet);

                dBContext.Entry(model).State = EntityState.Modified;
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
            Category model = (from category in dBContext.Categories
                              where id.Equals(category.CategoryId)
                              select category).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id,FormCollection collection)
        {
            Category model = (from category in dBContext.Categories
                              where id.Equals(category.CategoryId)
                              select category).FirstOrDefault();

            dBContext.Categories.Remove(model);
            dBContext.SaveChanges();
            return RedirectToAction("List");
        }

        public ActionResult getCategoryList()
        {
            List<Category> list = dBContext.Categories.ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }

     
    }
}